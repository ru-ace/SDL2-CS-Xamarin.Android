# SDL2-CS-Xamarin.Android

## Difference from [SDL2Droid-CS](https://github.com/0x0ade/SDL2Droid-CS)

* Switch to SDL2 2.0.8 and Android API Level 19
* Precompiled bundle of shared SDL2 libs for `arm64-v8a`, `armeabi-v7a`, `x86`, `x86_64` from my submodule [SDL2-CS-libs-bundle](https://github.com/ru-ace/SDL2-CS-libs-bundle):
    * `SDL2` 2.0.8 - cause it needs API Level 19 (2.0.9 wants >= 26)
    * `SDL2_image` 2.0.4
    * `SDL2_mixer` 2.0.4
    * `SDL2_ttf` 2.0.15
* Precompiled `SDL2Droid-CS-Java.jar` (src from SDL2 2.0.8)
* Simple test code for SDL2_* libs.
* Removed `dep/SDL` submodule. Broke `SDL2Droid-CS-Native`: instructions for manual build of libs bundle could be found at [SDL2-CS-libs-bundle](https://github.com/ru-ace/SDL2-CS-libs-bundle)

Please keep in mind that this repo contains *proof of concept* with very dirty code and **without any support**. 
I used it to understand: in future i will be able to port my project with SDL2_* libs to android.

If you want test this by yourself, please dont forget about submodules:
```
git clone --recursive https://github.com/ru-ace/SDL2-CS-Xamarin.Android
```

## Credits

* Original repo. Really great work! - https://github.com/0x0ade/SDL2Droid-CS 
* Examples for [SDL2-CS](https://github.com/flibitijibibo/SDL2-CS/) - https://github.com/expert4pro/SharpSdl2Examples   

------

# Original readme of SDL2Droid-CS
### An opera in three parts: SDL2 + SDL2-CS + Xamarin.Android
#### zlib-licensed
#### clone recursively
----

#### TL;DR:
* Run `/SDL2Droid-CS-Native/buildnative.sh`
* Compile `/SDL2Droid-CS-Java/` into `/SDL2Droid-CS-JBindings/Jars/SDL2Droid-CS-Java.jar`
* Change `/SDL2Droid-CS/Bootstrap.cs`, `/SDL2Droid-CS/Resources/` and `/SDL2Droid-CS/MainActivity.cs`
* Compile SDL2Droid-CS using Xamarin.Android (f.e. via Xamarin for Visual Studio).

#### /SDL2Droid-CS-Native/

*What:* SDL2 `Android.mk` makefile and "wrapper" code (SDL_main returning to C# managed land)

*Why:* Xamarin for Visual Studio has got some problems with compiling native libraries on its own.

*Compilation:*Run `buildnative.sh` (cygwin-compatible) from inside `/SDL2Droid-CS-Native/`. That's it.

#### /SDL2Droid-CS-Java/

*What:* Original SDL2 `SDLActivity.java` interoperating with native SDL2 code

*Why:* SDL2's native side is prepared for the JNI bindings in `SDLActivity.java` - let's just reuse them!

*Compilation:* In your favourite Java IDE, produce a compiled .jar artifact and place it into `/SDL2Droid-CS-JBindings/Jars/`

#### /SDL2Droid-CS/ and /SDL2Droid-CS-JBindings/

*What:* The Mono side of things: Main C# code and SDLActivity bindings.

*Why:* Why not?

*Compilation:*
* Load the solution in Visual Studio with the "Xamarin for Visual Studio" extension installed.
* Modify `/SDL2Droid-CS/Bootstrap.cs` for your main code, `/SDL2Droid-CS/Resources/` for any strings / icons / ... and `/SDL2Droid-CS/MainActivity.cs` for the launch config (f.e. orientation).
* Compile as you would any other Xamarin.Android project.
