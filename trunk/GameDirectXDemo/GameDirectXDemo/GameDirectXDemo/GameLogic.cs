using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput; //Added to take care of the KeyboardState and enum Key
using GameDirectXDemo.Core;
using System.Drawing;
using GameDirectXDemo.Manager;

namespace GameDirectXDemo
{
    /// <summary>
    /// This class controls the main game flow and the
    /// interaction between all other game classes.
    /// </summary>

    /// <summary>
    /// Holds the various game states.
    /// </summary>
    public enum GameStates
    {
        Run = 0,
        Exit = 1
    }

    class GameLogic
    {
        protected Control target;
        protected DxInitGraphics graphics;
        protected GameStates gameState;
        protected DxKeyboard input;
        protected GameManager gameManager;
        protected DxMouse mouse;
        protected double dLoopDuration; // Duration of one game loop in milliseconds.
        /// <summary>
        /// Constructor. Initializes the general graphics 
        /// and starts the game loop.
        /// </summary>
        /// <param name="RenderTarget">The target control to render to.</param>
        /// 

        DxTileMap tileMap;
        DxCamera camera;

        public GameLogic(Control RenderTarget)
        {
            // Save a reference to the target Control
            this.target = RenderTarget;

            // Add an eventhandler for the GotFocus event
            this.target.GotFocus += new System.EventHandler(this.restore);

            // Create a new graphics handler
            this.graphics = new DxInitGraphics(this.target);

            // Create a new input handler
            this.input = new DxKeyboard(this.target);

            this.mouse = new DxMouse(this.graphics,this.target, this);
            

            //Create a game Manager
            this.gameManager = new GameManager(this.target, this.graphics);
            // All done - set the game state to initialized
            this.gameState = GameStates.Run;

            try
            {
                // Initialize TimerEngine
                DxTimer.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while initializing: " + ex.Message);
                return;
            }

           // tileMap = new DxTileMap("Map/map2.txt", "Map/map.png", 32, 32, graphics);
           // camera = new DxCamera(new Point(0, 0), new Size(320, 320), tileMap.TileMapSurface, graphics.DDDevice);
            // Start game loop
            this.gameLoop();
        }

        /// <summary>
        /// This method handles the GotFocus event of the render form.
        /// If it runs fullscreen and loses focus, the device gets 
        /// not restored when switching back. So this has to be done
        /// manually.
        /// </summary>
        /// <param name="Sender">The sender (main form here)</param>
        /// <param name="e">EventParameter</param>
        protected void restore(object Sender, System.EventArgs e)
        {
            // Recreate graphics handler
            graphics.CreateSurfaces();
            this.graphics = new DxInitGraphics(this.target);
        }


        /// <summary>
        /// Retrieves the input state from the input handler
        /// and reacts depending on the user input.
        /// </summary>
        protected void processInput()
        {
            // This will save the current keyboard state
            KeyboardState state;

            // Get the keyboard state
            state = this.input.GetKeyboardState();

            // process the keyboard state
            if (state != null)
            {
                // on escape -> exit
                if (state[Key.Escape])
                {
                    gameState = GameStates.Exit;
                }
            }
        }


        /// <summary>
        /// This method controls the main game loop. While the target
        /// control is created, it will render the game content to it.
        /// </summary>
        protected void gameLoop()
        {
            // To check the elapsed time
            dLoopDuration = 0.0;

            // Start Timer
            DxTimer.Start();

            // Check if target control is still available
            while (target.Created)
            {
                // If the target control has got no focus,
                // there's no need to render or calculate the
                // game parameters.
                #region Lost focus. Don't care about it
                //if (!target.Focused)
                //{
                //    // Let the thread sleep a bit
                //    System.Threading.Thread.Sleep(100);

                //    // Allow the application to handle Windows messages
                //    Application.DoEvents();

                //    // Get elapsed time (to "reset" timer and therefore freeze gameplay)
                //    DxTimer.GetElapsedMilliseconds();

                //    // Do the next loop
                //    continue;
                //}
                #endregion

                //300px / 1s = 0.3px / 1ms
                if (dLoopDuration > 33.333)//50
                {
                   

                    #region IPO
                    // React on user input
                    this.processInput();

                    // If the gameState is set to Run
                    if (gameState == GameStates.Run)
                    {
                        //Remember GraphicsEngine RenderSurface method gets the Secondary Surface

                        //graphics.RenderSurface.ColorFill(Color.Aqua);
                        //graphics.RenderSurface.ForeColor = System.Drawing.Color.Red;

                        //ani.Update(dLoopDuration);
                        //ani.Draw(graphics.RenderSurface);
                       // this.mouse.Draw();

                        //graphics.RenderSurface.DrawText(100, 100, "Ticks per second: " + DxTimer.TicksPerSecond.ToString(), false);
                       // graphics.RenderSurface.DrawText(100, 115, "Frames per second: " + (1000.0 / dLoopDuration).ToString("F2"), false);
                       // graphics.RenderSurface.DrawText(100, 130, "Loop Duration (ms): " + dLoopDuration.ToString("F5"), false);

                        this.Update(dLoopDuration);
                       // graphics.RenderSurface.DrawText(100, 140, "Speed (px): " + (dLoopDuration * 0.3).ToString("F5"), false);

                        this.Draw();
                        //tileMap.DrawTileMap(graphics.RenderSurface);
                        //camera.Draw(graphics.RenderSurface);
                       // this.mouse.Draw();  

                    }


                    // If the gameState is set to exit, shut down game
                    if (gameState == GameStates.Exit)
                    {
                        return;
                    }
                  //  mouse.DrawMouse(this.graphics);
                    // Draw everything to the screen
                    graphics.Flip(); // Flip the secondary to the primary surface.
#endregion
                    dLoopDuration = 0.0;
                }//9

                dLoopDuration += DxTimer.GetElapsedMilliseconds();            


                // Allow application to handle Windows messages
                Application.DoEvents();
             }
        }

        private void Update(double dLoopDuration)
        {
            try 
            {
                mouse.Update();
                gameManager.Update(dLoopDuration, input.GetKeyboardState(), mouse.GetMouseState());
                //mouse.getMouseState();
                
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
            }
        }

        private void Draw()
        {
            try
            {
                gameManager.Draw(this.graphics);
                mouse.DrawMouse(this.graphics);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    
    }
}
