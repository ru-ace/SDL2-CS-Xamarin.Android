using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SDL2;

using System.Runtime.InteropServices;
using Android.Content.Res;
using Android.Util;
using Xamarin.Android;


namespace SDL2Droid_CS
{
    delegate void Main();


    static class Bootstrap
    {

        public static void SDL_Main()
        {
            // Example code.

            // OPTIONAL: Hide action bar (top bar). Otherwise it just shows the window title.
            // MainActivity.Instance.RunOnUiThread(MainActivity.Instance.ActionBar.Hide);

            // OPTIONAL: Fullscreen (immersive), handled by the activity
            MainActivity.SDL2DCS_Fullscreen = true;

            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
            else
                Console.WriteLine("SDL initialized!");

            // OPTIONAL: init SDL_image
            var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG | SDL_image.IMG_InitFlags.IMG_INIT_WEBP;
            if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                Console.WriteLine("SDL_image could not initialize! SDL_image Error: {0}", SDL.SDL_GetError());
            else
                Console.WriteLine("SDL_image initialized!");

            // OPTIONAL: init SDL_ttf

            if (SDL_ttf.TTF_Init() == -1)
                Console.WriteLine("SDL_ttf could not initialize! SDL_image Error: {0}", SDL.SDL_GetError());
            else
                Console.WriteLine("SDL_ttf initialized!");

            // OPTIONAL: init SDL_mixer
            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
                Console.WriteLine("SDL_mixer could not initialize! SDL_image Error: {0}", SDL.SDL_GetError());
            else
                Console.WriteLine("SDL_mixer initialized!");

            // OPTIONAL: Get WM. Required to set the backbuffer size to the screen size
            DisplayMetrics dm = new DisplayMetrics();
            MainActivity.SDL2DCS_Instance.WindowManager.DefaultDisplay.GetMetrics(dm);

            if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1") == SDL.SDL_bool.SDL_FALSE)
                Console.WriteLine("Warning: Linear texture filtering not enabled!");


            IntPtr window = SDL.SDL_CreateWindow(
                "HEY, LISTEN!",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                dm.WidthPixels,
                dm.HeightPixels,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL |
                SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS |
                SDL.SDL_WindowFlags.SDL_WINDOW_MOUSE_FOCUS
            );

            if (window == IntPtr.Zero)
                Console.WriteLine("Window could not be created! SDL_Error: {0}", SDL.SDL_GetError());
            else
                Console.WriteLine("Window created!");

            //Test code

            TestCode tc = new TestCode(window, dm.WidthPixels, dm.HeightPixels);

            //selector
            const bool gl_test = false;

            if (gl_test)
            {
                //GL Test Code
                tc.GL_Init();
                tc.GL_Loop();
            }
            else
            {
                //Test of SDL_image, SDL_ttf, SDL_mixer
                tc.Init();
                tc.Loop();
            }

            SDL.SDL_Quit();

        }

        public static void SetupMain()
        {
            // Give the main library something to call in Mono-Land afterwards
            SetMain(SDL_Main);
            // Insert your own post-lib-load, pre-SDL2 code here.
        }

        [DllImport("main")]
        static extern void SetMain(Main main);

    }
}
