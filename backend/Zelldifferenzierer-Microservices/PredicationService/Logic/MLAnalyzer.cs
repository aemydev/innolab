using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.Math.Optimization.Losses;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Statistics.Kernels;
using Accord.Statistics.Models.Regression.Fitting;
using Keras.Layers;
using Microsoft.AspNetCore.HttpOverrides;
using Tensorflow.Keras.Losses;
using System.Drawing.Imaging;
using Accord.IO;

namespace PredictionService.Logic
{
    public class MLAnalyzer
    {
        public void TrainAnglePredicationStraightModel(string[] pathsToImages, string[] pathsToLabels)
        {
            // Load the labeled myocyte images and corresponding angle labels
            double[][] inputs; // Input images
            double[] outputs; // Angle labels

            var res = LoadMyocyteImages(pathsToImages, pathsToLabels);

            inputs = res.Item1;
            outputs = res.Item2;

            // Split the data into training and validation sets
            int trainSize = (int)(inputs.Length * 0.8);
            int validationSize = inputs.Length - trainSize;
            double[][] trainInputs = new double[trainSize][];
            double[][] trainOutputs = new double[trainSize][];
            double[] validationInputs = new double[validationSize];
            double[] validationOutputs = new double[validationSize];
            Array.Copy(inputs, 0, trainInputs, 0, trainSize);
            Array.Copy(outputs, 0, trainOutputs, 0, trainSize);
            Array.Copy(inputs, trainSize, validationInputs, 0, validationSize);
            Array.Copy(outputs, trainSize, validationOutputs, 0, validationSize);

            // Define the U-Net architecture
            ActivationNetwork network =
                new ActivationNetwork(new SigmoidFunction(), inputs[0].Length, 32, 64, 128, 64, 32, 1);

            // Compile the model
            LevenbergMarquardtLearning teacher = new LevenbergMarquardtLearning(network);

            int epochs = 10;

            // Train the model
            for (int i = 0; i < epochs; i++)
            {
                double error = teacher.RunEpoch(trainInputs, trainOutputs);
                Console.WriteLine("Epoch {0}: error = {1}", i, error);
            }

            // Evaluate the model on the validation data

            double validationError = new SquareLoss(validationOutputs).Loss(network.Compute(validationInputs));

            // Save the model to a file
            network.Save("myocyte_angle_predictor.bin");
        }

        public double PredictAngle(string pathToImage)
        {
            ActivationNetwork network;
            if (!File.Exists("myocyte_angle_predictor.bin") || !File.Exists(pathToImage)) throw new FileNotFoundException();
            using (var stream = new FileStream("myocyte_angle_predictor.bin", FileMode.Open))
            {
                network = Serializer.Load<ActivationNetwork>(stream);
            }

            double[] input = ImageToPixelValues(new Bitmap(pathToImage));
            return network.Compute(input)[0];
        }

        private static Tuple<double[][], double[]> LoadMyocyteImages(string[] imageFilePaths, string[] angleFilePaths)
        {
            double[][] inputs = new double[imageFilePaths.Length][];
            double[] outputs = new double[angleFilePaths.Length];

            for (int i = 0; i < imageFilePaths.Length; i++)
            {
                // Load the image
                var image = new Bitmap(imageFilePaths[i]);

                // Convert the image to an array of pixel values
                inputs[i] = ImageToPixelValues(image);

                // Load the corresponding angle label
                outputs[i] = double.Parse(File.ReadAllText(angleFilePaths[i]));
            }

            return new(inputs, outputs);
        }

        private static double[] ImageToPixelValues(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            int stride = width * 4; // 4 bytes per pixel (32-bit RGBA)
            int pixelCount = width * height;

            // Create a 1D array to store the pixel values
            double[] pixelValues = new double[pixelCount * 4];

            // Lock the bits of the image
            BitmapData data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy the pixels to the array
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixelValues, 0, pixelCount * 4);

            // Unlock the bits of the image
            image.UnlockBits(data);

            // Normalize the pixel values
            for (int i = 0; i < pixelCount * 4; i++)
            {
                pixelValues[i] /= 255;
            }

            return pixelValues;
        }
    }
}
