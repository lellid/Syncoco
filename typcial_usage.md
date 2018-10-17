---
layout: page
title: Typical usage
permalink: /typical_usage.html
---

### Is Syncoco useful for you?

- You have two computers (work, home) with no or very limited (slow) network connection.
- You want to keep your files synchronized between both computers.
- The total size of all files you want to keep synchronized does not fit on the transfer medium you have (for instance a memory stick).
- The total size of all files that have changed in one session (between synchronizations) is typically less than the size of the transfer medium.

### Typical usage scenario

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
And that's all! For details how to change synchronization actions, see the manual.

If your work on `Computer B` is then finished after some time,
repeat the steps from the beginning!

### Pro tip:

To open Syncoco.exe with your synchronization file, open the file explorer with your USB memory stick location, and
drag the synchronization file onto the `Syncoco.exe` file. This opens `Syncoco.exe`automatically with your synchronization file.





