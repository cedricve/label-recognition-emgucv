Label Recognition at ArcelorMittal
==================================

This application is a tool created for my master thesis to prevent stock swaps in the production halls of ArcelorMittal. Image Processing (OpenCV, EmguCV) is used to automatically locate the ID of the material and OCR (Tesseract) to recognize the ID.

![alt text](http://www.cedricve.me/wp-content/uploads/2013/07/K00037722979S_31_07_2013-14_02_20-e1375278547128.jpg "Material")&nbsp;![alt text](http://www.cedricve.me/wp-content/uploads/2013/07/K00037785642S_31_07_2013-13_52_16-e1375278470382.jpg "Material")

More information can be found on: www.cedricve.me/master-thesis

Installation
========================

1. After you cloned the directory you have to install EmguCV (http://sourceforge.net/projects/emgucv/files/emgucv/2.3.0/libemgucv-windows-x86-2.3.0.1416.exe/download).
2. Open the solution.
  1. Right click on References, select Add Reference.
  2. Browse to your EmguCV directory (default C:\Emgu).
  3. Select your EmguCV version (C:\Emgu\emgucv-windows-x86 2.3.0.1416).
  4. Open the bin directory.
  5. Select all dll's with prefix "Emgu.CV.".
3. Open you file browser and go the same bin directory (C:\Emgu\emgucv-windows-x86 2.3.0.1416\bin\).
  1. Select all files with prefix "opencv_"  and the files: "npp32_40_17.dll", "cvextern.dll", "cvextern_gpu.dll","cufft32_40_17.dll", "cudart32_40_17.dll" and "ZedGraph.dll".
  2. Drag the files in the solution.
  3. Select all files and open properties, change the value of "Copy To Output" to "Copy Always".
4. Move the tessdata directory to your C:\ drive. (or to another directory you prefer but then you need to edit the path in the source code).
5. Run the project and select an image from the example directory.
