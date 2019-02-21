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
    // Delegates for the example GL code
    delegate void DglClearColor(
        float red,
        float green,
        float blue,
        float alpha
    );
    delegate void DglClear(int mask);


    public class TestCode
    {
        private IntPtr window = IntPtr.Zero;
        private int window_width = 0;
        private int window_height = 0;

        private IntPtr renderer = IntPtr.Zero;

        private DglClearColor glClearColor;
        private DglClear glClear;


        public TestCode(IntPtr window, int window_width, int window_height)
        {
            this.window = window;
            this.window_width = window_width;
            this.window_height = window_height;

        }


        public void GL_Init()
        {
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_SHARE_WITH_CURRENT_CONTEXT, 1);

            IntPtr glContext = SDL.SDL_GL_CreateContext(window);
            SDL.SDL_GL_MakeCurrent(window, glContext);

            SDL.SDL_DisableScreenSaver();

            glClearColor = (DglClearColor)Marshal.GetDelegateForFunctionPointer(
                SDL.SDL_GL_GetProcAddress("glClearColor"),
                typeof(DglClearColor)
            );
            glClear = (DglClear)Marshal.GetDelegateForFunctionPointer(
                SDL.SDL_GL_GetProcAddress("glClear"),
                typeof(DglClear)
            );
        }
        public void GL_Loop()
        {
            DateTime start = DateTime.UtcNow;

            SDL.SDL_Event evt;
            DateTime now;
            TimeSpan span;

            bool exit = false;


            while (!exit)
            {
                while (SDL.SDL_PollEvent(out evt) == 1)
                {
                    if (evt.type == SDL.SDL_EventType.SDL_QUIT)
                        exit = true;

                }

                now = DateTime.UtcNow;
                span = now - start;

                float t = (float)(Math.Sin(span.TotalSeconds) * 0.5 + 0.5);

                glClearColor(1 - t, t, t, 1f);
                glClear(0x4000); // GL_COLOR_BUFFER_BIT

                SDL.SDL_GL_SwapWindow(window);

            }
        }



        public void Init()
        {

            //IntPtr surface = SDL.SDL_GetWindowSurface(window);
            //if (surface == IntPtr.Zero)
            //Console.WriteLine("Surface could not be created! SDL Error: {0}", SDL.SDL_GetError());

            renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            if (renderer == IntPtr.Zero)
                Console.WriteLine("Renderer could not be created! SDL Error: {0}", SDL.SDL_GetError());

            SDL.SDL_SetRenderDrawColor(renderer, 0xFF, 0xFF, 0xFF, 0xFF);

            Console.WriteLine("Base Path: {0}", SDL.SDL_GetBasePath());
            Console.WriteLine("ApplicationData Path: {0}", System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));

            //LoadResources();
        }

        private void LoadResources()
        {


            //Load music
            //string filename = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "beat.wav");
            IntPtr music = SDL_mixer.Mix_LoadMUS("file://android_asset/beat.wav");
            if (music == IntPtr.Zero)
                Console.WriteLine("Failed to load! {0}", SDL.SDL_GetError());


        }
        public void Loop()
        {
            bool exit = false;
            SDL.SDL_Event e;

            while (!exit)
            {
                while (SDL.SDL_PollEvent(out e) == 1)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        exit = true;

                }

                //Clear screen
                SDL.SDL_SetRenderDrawColor(renderer, 0xFF, 0xFF, 0xFF, 0xFF);
                SDL.SDL_RenderClear(renderer);


                //Render red filled quad
                var fillRect = new SDL.SDL_Rect { x = window_height / 4, y = window_height / 4, w = window_height / 2, h = window_height / 2 };
                SDL.SDL_SetRenderDrawColor(renderer, 0xFF, 0x00, 0x00, 0xFF);
                SDL.SDL_RenderFillRect(renderer, ref fillRect);

                //Render green outlined quad
                var outlineRect = new SDL.SDL_Rect { x = window_height / 6, y = window_height / 6, w = window_height * 2 / 3, h = window_height * 2 / 3 };
                SDL.SDL_SetRenderDrawColor(renderer, 0x00, 0xFF, 0x00, 0xFF);
                SDL.SDL_RenderDrawRect(renderer, ref outlineRect);

                //Draw blue horizontal line
                SDL.SDL_SetRenderDrawColor(renderer, 0x00, 0x00, 0xFF, 0xFF);
                SDL.SDL_RenderDrawLine(renderer, 0, window_height / 2, window_height, window_height / 2);

                //Draw vertical line of yellow dots
                SDL.SDL_SetRenderDrawColor(renderer, 0xFF, 0xFF, 0x00, 0xFF);
                for (int i = 0; i < window_height; i += 4)
                {
                    SDL.SDL_RenderDrawPoint(renderer, window_height / 2, i);
                }

                //Update screen
                SDL.SDL_RenderPresent(renderer);
                SDL.SDL_Delay(50);
            }
        }

    }
}
