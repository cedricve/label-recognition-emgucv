Label Recognition at ArcelorMittal
==================================

This application is a tool created for my master thesis to prevent stock swaps in the production halls of ArcelorMittal. Image Processing (OpenCV, EmguCV) is used to automatically locate the ID of the material and OCR (Tesseract) to recognize the ID.
More information can be found on: www.cedricve.me/master-thesis

Installation
========================

1. After you cloned the directory you have to install EmguCV (http://sourceforge.net/projects/emgucv/files/emgucv/2.3.0/libemgucv-windows-x86-2.3.0.1416.exe/download).
2. Open the solution.
  1. Right click on References, select Add Reference.
  2. Search for your EmguCV directory (default C:\Emgu).
  3. Select your EmguCV version (C:\Emgu\emgucv-windows-x86 2.3.0.1416).
  4. Open the bin directory.
  5. Select all dll's with prefix "Emgu.CV.".
3. Open you file browser and go the same bin directory (C:\Emgu\emgucv-windows-x86 2.3.0.1416\bin\).
  1. Select all files with prefix "opencv_"  and the files: "npp32_40_17.dll", "cvextern.dll", "cvextern_gpu.dll","cufft32_40_17.dll", "cudart32_40_17.dll".
  2. Drag the files in the solution.
  3. Select all files and open properties, change the value of "Copy To Output" to "Copy Always".
4. Move the tessdata directory to your C:\ drive. (or to another directory you prefer but then you need to edit the path in the source code).
5. Run the project and select an image from the example directory.
