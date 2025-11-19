# Syncoco

Syncoco is a very special and very useful file synchronization program, using a USB memory stick.

Ever had the problem that you want to synchronize files between two computers, which are either:
- not connected by a network
- connected, but the connection is slow
- you have an aweful lot of files, so it would take days to sync your files
- connected, but you don't want to transfer your data over that wire

# Installation

No installation required! Just download the latest Syncoco.exe file.  
(For the curious: Syncoco is a single file .NET 10 application, written in C#.)

# Typical usage

*Assuming*, you have two computers: `Computer A` and `Computer B`.
Syncoco.exe and the synchronization file (extension .syncoco) have been copied onto a USB memory stick, that is currently inserted in `Computer A`. 

If you have finished your work on `Computer A`,
 open `Syncoco.exe` with your synchonization file (see Pro tip below),
and choose `Ending work -> UpdateSaveAndCopyFiles` from the menu.
This let `Syncoco` go through all the directories you have configured for synchronization,
compares them with the file system state that is stored in the synchronization file, updates the synchronization file with the current state of the file system,
and stores all files that have been added onto the USB memory stick.
All automatically! Release the stick then.

Start working on `Computer B`. Insert the memory stick, open Syncoco with the synchronization file,
and choose `Beginning work -> Show files to sync` from the menu.
This will show up a list with all files to sync. It is **always a good idea** to have
a **look at the list**, because Syncoco synchronizes not only added files, 
but also **deleted** files! So, if you have deleted files on `Computer A`, they are just about to be deleted on
`Computer B`, too!! So make sure this is what you want!
If sure, choose `Beginning work -> Sync selected files`. If nothing was selected by you, 
Syncoco assumes that all files should be synchronized.
And that's all! For details how to change synchronization actions, see the [manual](https://lellid.github.io/Syncoco/help/html/7f39dda1-b3c1-4e94-9407-6006870f1b41.htm).

If your work on `Computer B` is then finished after some time,
repeat the steps from the beginning!

### Pro tip:

To open Syncoco.exe with your synchronization file, open the file explorer with your USB memory stick location, and
drag the synchronization file onto the `Syncoco.exe` file. This opens `Syncoco.exe`automatically with your synchronization file.

# Further information

For further information, please have a look at the [project page](https://lellid.github.io/Syncoco/).