using pco;
using System;
using System.Threading;

namespace MultiCameraExample
{
  class MultiCameraExample
  {
    static void Main(string[] args)
    {
      uint err = 0;
      string filename1;
      string filename2;
      string filenameRaw1 = "example_raw_1.tif";
      string filenameRaw2 = "example_raw_2.tif";
      string path = "";
      string saveType1;
      string saveType2;
      string saveTypeRaw = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_16;
      bool isBitmap1;
      bool isBitmap2;
      Image img1 = new Image();
      Image img2 = new Image();
      int image_count = 10;
      Camera cam1 = new Camera();
      Camera cam2 = new Camera();
      IntPtr sdkHandle = cam1.sdk;
      cam1.defaultConfiguration();
      cam1.setExposureTime(0.01);
      cam2.defaultConfiguration();
      cam2.setExposureTime(0.01);

      if (cam1.isColored())
      {
        filename1 = "example_1.bmp";
        isBitmap1 = true;
        saveType1 = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BGR_8;
      }
      else
      {
        filename1 = "example_1.tif";
        isBitmap1 = false;
        saveType1 = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_8;
      }

      if (cam2.isColored())
      {
        filename2 = "example_2.bmp";
        isBitmap2 = true;
        saveType2 = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BGR_8;
      }
      else
      {
        filename2 = "example_2.tif";
        isBitmap2 = false;
        saveType2 = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_8;
      }


      cam1.record(image_count, pco.RecordMode.sequence);
      cam2.record(image_count, pco.RecordMode.sequence);

      Console.WriteLine("Cam 1 recorded image count:" + cam1.getRecordedImageCount());
      Console.WriteLine("Cam 2 recorded image count:" + cam2.getRecordedImageCount());
      
      for (uint counter = 0; counter < image_count; counter++)
      {
        // Save cam1
        // ==============================================
        if (cam1.isColored())
        {
          cam1.image(img1, counter, null, DataFormat.BGR8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img1.vector_8bit(), (UInt16)img1.width(), (UInt16)img1.height(), saveType1, isBitmap1, path + counter + "_" + filename1, true, IntPtr.Zero);
        }
        else
        {
          cam1.image(img1, counter, null, DataFormat.Mono8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img1.vector_8bit(), (UInt16)img1.width(), (UInt16)img1.height(), saveType1, isBitmap1, path + counter + "_" + filename1, true, IntPtr.Zero);
        }
        if (err != 0)
          throw new Camera_Exception(err);

        // Save also raw image
        err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img1.raw_vector_16bit(), (UInt16)img1.width(), (UInt16)img1.height(), saveTypeRaw, false, path + counter + "_" + filenameRaw1, true, IntPtr.Zero);

        if (err != 0)
          throw new Camera_Exception(err);
        // ==============================================

        // Save cam2
        // ==============================================
        if (cam2.isColored())
        {
          cam2.image(img2, counter, null, DataFormat.BGR8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img2.vector_8bit(), (UInt16)img2.width(), (UInt16)img2.height(), saveType2, isBitmap2, path + counter + "_" + filename2, true, IntPtr.Zero);
        }
        else
        {
          cam2.image(img2, counter, null, DataFormat.Mono8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img2.vector_8bit(), (UInt16)img2.width(), (UInt16)img2.height(), saveType2, isBitmap2, path + counter + "_" + filename2, true, IntPtr.Zero);
        }
        if (err != 0)
          throw new Camera_Exception(err);

        // Save also raw image
        err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img2.raw_vector_16bit(), (UInt16)img2.width(), (UInt16)img2.height(), saveTypeRaw, false, path + counter + "_" + filenameRaw2, true, IntPtr.Zero);

        if (err != 0)
          throw new Camera_Exception(err);
        // ==============================================

      }
      //We suggest to actively close the cameras to avoid garbage collection errors
      cam1.close();
      cam2.close();
    }
  }
}
