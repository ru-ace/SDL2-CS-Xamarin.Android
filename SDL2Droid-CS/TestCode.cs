using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
//using Android.OS;
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


            LoadAndPrepareResources();
        }

        private IntPtr music_mp3;
        private IntPtr music_wav;
        private IntPtr font;
        private IntPtr texture_png;
        private SDL.SDL_Surface s_png;
        private IntPtr texture_text;
        private SDL.SDL_Surface s_text;

        private void LoadAndPrepareResources()
        {


            music_mp3 = SDL_mixer.Mix_LoadMUS("ringtone.mp3");
            if (music_mp3 == IntPtr.Zero)
                Console.WriteLine("Failed to load mp3! SDL_mixer error {0}", SDL.SDL_GetError());

            SDL_mixer.Mix_PlayMusic(music_mp3, -1);

            music_wav = SDL_mixer.Mix_LoadWAV("beat.wav");
            if (music_wav == IntPtr.Zero)
                Console.WriteLine("Failed to load wav! SDL_mixer error {0}", SDL.SDL_GetError());

            font = SDL_ttf.TTF_OpenFont("lazy.ttf", 64);
            if (font == IntPtr.Zero)
                Console.WriteLine("Failed to load lazy font! SDL_ttf Error: {0}", SDL.SDL_GetError());

            //Render text surface
            var textColor = new SDL.SDL_Color();
            var textSurface = SDL_ttf.TTF_RenderText_Solid(font, "Hello SDL2 World!", textColor);
            if (textSurface == IntPtr.Zero)
                Console.WriteLine("Unable to render text surface! SDL_ttf Error: {0}", SDL.SDL_GetError());

            texture_text = SDL.SDL_CreateTextureFromSurface(renderer, textSurface);
            if (texture_text == IntPtr.Zero)
                Console.WriteLine("Unable to create texture from rendered text! SDL Error: {0}", SDL.SDL_GetError());
            s_text = Marshal.PtrToStructure<SDL.SDL_Surface>(textSurface);

            IntPtr image_png = SDL_image.IMG_Load("transparent.png");
            if (image_png == IntPtr.Zero)
                Console.WriteLine("Unable to load image! SDL_image Error: {0}", SDL.SDL_GetError());

            s_png = Marshal.PtrToStructure<SDL.SDL_Surface>(image_png);
            SDL.SDL_SetColorKey(image_png, (int)SDL.SDL_bool.SDL_TRUE, SDL.SDL_MapRGB(s_png.format, 0, 0xFF, 0xFF));

            texture_png = SDL.SDL_CreateTextureFromSurface(renderer, image_png);
            if (texture_png == IntPtr.Zero)
                Console.WriteLine("Unable to create texture from png! SDL Error: {0}", SDL.SDL_GetError());






        }
        public void Loop()
        {
            bool exit = false;
            SDL.SDL_Event e;
            double angle = 0;
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
                var fillRect = new SDL.SDL_Rect { x = window_width / 4, y = window_height / 4, w = window_width / 2, h = window_height / 2 };
                SDL.SDL_SetRenderDrawColor(renderer, 0x00, 0xFF, 0x00, 0xFF);
                SDL.SDL_RenderFillRect(renderer, ref fillRect);

                //Render green outlined quad
                var outlineRect = new SDL.SDL_Rect { x = window_width / 6, y = window_height / 6, w = window_width * 2 / 3, h = window_height * 2 / 3 };
                SDL.SDL_SetRenderDrawColor(renderer, 0xFF, 0x00, 0x00, 0xFF);
                SDL.SDL_RenderDrawRect(renderer, ref outlineRect);

                //Draw blue horizontal line
                SDL.SDL_SetRenderDrawColor(renderer, 0x00, 0x00, 0xFF, 0xFF);
                SDL.SDL_RenderDrawLine(renderer, 0, window_height / 2, window_width, window_height / 2);

                //Draw vertical line of black dots
                SDL.SDL_SetRenderDrawColor(renderer, 0x00, 0x00, 0x00, 0xFF);
                for (int i = 0; i < window_height; i += 4)
                    SDL.SDL_RenderDrawPoint(renderer, window_width / 2, i);

                //var center = new SDL.SDL_Point { x = window_width / 2, y = window_height / 2 };


                //Render text
                var dst_rect = new SDL.SDL_Rect { x = window_width / 2 - s_text.w / 2, y = window_height / 2 - s_text.h / 2, w = s_text.w, h = s_text.h };
                SDL.SDL_RenderCopyEx(renderer, texture_text, IntPtr.Zero, ref dst_rect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);

                //Render png
                dst_rect = new SDL.SDL_Rect { x = window_width / 2 - s_png.w / 2, y = window_height / 6 - s_png.h / 2, w = s_png.w, h = s_png.h };
                SDL.SDL_RenderCopyEx(renderer, texture_png, IntPtr.Zero, ref dst_rect, 0, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);

                //Update screen
                SDL.SDL_RenderPresent(renderer);
                SDL.SDL_Delay(50);
                angle++;
                angle = angle > 360 ? 0 : angle;
            }
        }

    }
}
