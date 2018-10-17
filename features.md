---
layout: page
title: Features of Syncoco
permalink: /features.html
---

Syncoco (Synchronize two computers) is a file synchronization tools that synchronizes one or more directories between two computers (see below: “Why only two computers?”). Unless other file synchronization tools, it
***does not need an online connection*** between the two computers
not only copies files, but ***reply all actions*** you made on one computer to the other computer, i.e. it can create, move and delete files.
Instead of using a online connection, a data medium (e.g. an USB stick, CD-RW or mobile device) is used for synchronization. The **size of this transfer medium** can be **much smaller than the size of the directory structure** you want to keep synchronized, since only changed/missing files are transferred.

### What is currently implemented?
- Can handle creating new files, deleting files, and movement of files.
- Keeps one or more directories (including subdirectories) of two different computers synchronized. The directories can have different names on both computers.
- Not necessary to have a direct connection between the computers, the synchronization is performed by a data medium (memory stick, CD-RW).
- Powerful file filtering system allows you to synchronize exactly that files you want to synchronize. Exclude files that are only of temporary nature or specially tailored for one computer.
- File system type (NTFS, FAT32, FAT) of the two computers can be different (although not thoroughly tested). The file system of the transfer medium can also differ (it must only support file names with more than 8 characters)
- Size of the directories can be much larger than the size of the transfer medium. Only changed/missing files are stored on the transfer medium. Additionally, the Syncoco document file itself is stored on the transfer medium. This file contains the directory and file structure state of the two computers.
- Transferring of files is verified by means of the MD5 hash sum.
 
### What is currently not implemented?
Files with a size larger than the transfer medium can not be transferred. There is no feature to part the file into smaller pieces for transferring.
Advanced file system features, like hard links and symbolic links, substreams and so on are not supported.
During synchonization of newly created files, the file attributes are synchronized too. However, if you later on change the attribute of a file (only the attribute), this change is not synchronized to the other system.
Permission settings of files and directories are not synchronized.

### Why synchronizing only two computers?
As mentioned above, only changed files are stored on the transfer medium. If you finished working on one computer, you typically start Syncoco and initiate the synchronization. In this process, Syncoco has to decide what files to store to the transfer medium. This is been done by comparing the state of the file system here with the file system there (on the other computer), and than copying files that have changed here or are missed there.
Of course, nobody prevents you from storing more than one Syncoco document on your transfer medium. But you must initiate a different synchronization for exactly this document.

