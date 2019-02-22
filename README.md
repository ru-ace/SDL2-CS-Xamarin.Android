# Difference from origin repo 

* Updated to SDL2 2.0.8 and works at this moment
* Contains precompiled bundle of shared SDL2 libs for arm64-v8a, armeabi-v7a, x86, x86_64:
    * SDL2 2.0.8 - cause it needs API Level 19 (2.0.9 wants >=26)
    * SDL2_image 2.0.4
    * SDL2_mixer 2.0.4
    * SDL2_ttf 2.0.15
    * Instruction for creating bundle: https://wiki.libsdl.org/Android (section 4.1). Dont forget add SDL2Droid-CS-Native/wrapper/* to SDL2/build/org.libsdl/app/jni/src/* and fix path in Android.mk  
* Precompiled SDL2Droid-CS-Java.jar (src from SDL2 2.0.8)
* Api Level 19
* Simple test code for SDL2_* libs.

Please keep in mind that this repo contains *proof of concept* with very dirty code and **without any support**. 
I used it to understand: in future i will be able to port my project with SDL2_* libs to android.

## Credits

* Original repo. Really great work! - https://github.com/0x0ade/SDL2Droid-CS 
* Test code examples - https://github.com/expert4pro/SharpSdl2Examples   

# SDL2Droid-CS
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

*Compilation:* Run `buildnative.sh` (cygwin-compatible) from inside `/SDL2Droid-CS-Native/`. That's it.

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
