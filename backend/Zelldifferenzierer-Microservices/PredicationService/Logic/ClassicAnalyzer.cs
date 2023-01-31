using System.Drawing;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace PredictionService.Logic
{
    public class ClassicAnalyzer
    {

        private const int THRESHOLD = 100;
        public static Tuple<Bitmap, List<float>> AngleCalculation(string imagepath)
        {
            Mat image = CvInvoke.Imread(imagepath, ImreadModes.Color);
            Mat green = new Mat(image.Rows, image.Cols, DepthType.Cv8U, 1);
            CvInvoke.ExtractChannel(image, green, 1);

            // Apply thresholding to segment the myocytes
            Mat thresholded = new Mat();
            CvInvoke.Threshold(green, thresholded, 150, 255, ThresholdType.Binary);

            // Find the contours of the myocytes
            VectorOfVectorOfPoint contours = new();
            CvInvoke.FindContours(thresholded, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            List<float> angles = new();

            // Loop through each contour
            for (int i = 0; i < contours.Size; i++)
            {
                // Get the moments of the contour
                var moments = CvInvoke.Moments(contours[i], false);

                // Get the centroid of the contour
                int cX = (int)(moments.M10 / moments.M00);
                int cY = (int)(moments.M01 / moments.M00);

                // Draw the centroid on the image
                CvInvoke.Circle(green, new Point(cX, cY), 3, new MCvScalar(0, 0, 255), -1);

                // Get the angle of the myocyte
                var ellipse = CvInvoke.FitEllipse(contours[i]);

                angles.Add(ellipse.Angle);
            }

            return new(green.ToBitmap(), angles);
        }

        public static Tuple<Bitmap, float> MeanAngleCalculation(string imagepath)
        {
            Mat image = CvInvoke.Imread(imagepath, ImreadModes.Color);
            Mat green = new Mat(image.Rows, image.Cols, DepthType.Cv8U, 1);
            CvInvoke.ExtractChannel(image, green, 1);

            // Apply thresholding to segment the myocytes
            Mat thresholded = new Mat();
            CvInvoke.Threshold(green, thresholded, 150, 255, ThresholdType.Binary);

            // Find the contours of the myocytes
            VectorOfVectorOfPoint contours = new();
            CvInvoke.FindContours(thresholded, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            List<float> angles = new();

            // Loop through each contour
            for (int i = 0; i < contours.Size; i++)
            {
                // Get the moments of the contour
                var moments = CvInvoke.Moments(contours[i], false);

                // Get the centroid of the contour
                int cX = (int)(moments.M10 / moments.M00);
                int cY = (int)(moments.M01 / moments.M00);

                // Draw the centroid on the image
                CvInvoke.Circle(green, new Point(cX, cY), 3, new MCvScalar(0, 0, 255), -1);

        // Get the angle of the myocyte
              if (contours[i].Size >= 5)
              {
                var ellipse = CvInvoke.FitEllipse(contours[i]);

                angles.Add(ellipse.Angle);
              }

            }

            return new(green.ToBitmap(), angles.Average());
        }

        public static Tuple<Bitmap, double> FusionIndexCalculation(string imagepath, int threshold = THRESHOLD)
        {

            Mat img = new Mat(imagepath, ImreadModes.Color);

            // Convert the image to grayscale
            Mat gray = new Mat();
            CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

            // Apply thresholding to segment the cells
            Mat thresh = new Mat();
            CvInvoke.Threshold(gray, thresh, 0, 255, ThresholdType.Otsu | ThresholdType.Binary);

            // Find the contours of the cells
            VectorOfVectorOfPoint contours = new();
            CvInvoke.FindContours(thresh, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            // Initialize variables to store the number of cells and the number of multinucleated cells
            int total_cells = 0;
            int multi_nucleated_cells = 0;

            // Iterate through the contours
            for (int i = 0; i < contours.Size; i++)
            {
                // Get the moments of the contour
                var moments = CvInvoke.Moments(contours[i]);
                // Get the centroid of the contour
                int cx = (int)(moments.M10 / moments.M00);
                int cy = (int)(moments.M01 / moments.M00);
                // Get the area of the contour
                double area = CvInvoke.ContourArea(contours[i]);

                // Check if the area is above a threshold (to eliminate noise)
                if (area > threshold)
                {
                    total_cells++;
                    // Apply a HoughCircles transform to detect the nuclei
                    var dec = new CudaHoughCirclesDetector(0, 1, 20, 50, 30, 0, 0);
                    var circles = dec.Detect(gray);
                    if (circles != null)
                    {
                        // Count the number of circles/nuclei
                        int num_nuclei = circles.Length;
                        if (num_nuclei > 1)
                        {
                            multi_nucleated_cells++;
                        }
                    }
                }
            }


            // Calculate the fusion index
            double fusion_index = multi_nucleated_cells / (double)total_cells;

            return new(thresh.ToBitmap(), fusion_index);
        }

    }
}
