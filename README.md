# RyuGSCMT
 Guest Shader Cache Manifest Tool for Nintendo Switch emulator [Ryujinx](https://ryujinx.org).
 
 ⚠️ Please note that the Ryujinx developer team does not provide support for this tool and has not actively been involved in the development of this tool.
 
 ⚠️ This can (and will) destroy your shader cache if used improperly. Make a backup before use.

## Instructions
1. Make sure that you have made backups of any shader caches involved.
2. Combine all shaders from **Guest** cache.zip files into one cache.zip file.
3. Place RyuGSCMT.exe in the same folder as the combined cache.zip file from step 2.
4. Double click RyuGSCMT.exe to run it.
5. You will be prompted to enter the Guest Shader Cache version. As of writing this, 1759 should be used for any recently used cache. See below for more details.
6. Press Enter to proceed.
7. Press Enter again to exit RyuGSCMT.
8. Copy the new cache.info and cache.zip to the **Guest** shader directory of your game, if necessary.
9. Delete any **OpenGL** (or other host graphics API) shader cache folder(s).
10. Run Ryujinx and launch your game. All the shaders should now be recompiled for your host system.

### Regarding Guest Shader Cache version
This version number can/will change based on the development of Ryujinx. To be on the safe side, use each shader cache you wish to merge, at least once with the current version of Ryujinx (so any potential updates can be applied). You can find the current `GuestCacheVersion` [here](https://github.com/Ryujinx/Ryujinx/blob/master/Ryujinx.Graphics.Gpu/Shader/Cache/CacheManager.cs).
