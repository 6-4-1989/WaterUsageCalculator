using System;
using System.Linq;
using System.Diagnostics;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

namespace WaterUsageBoilerplate.Models
{
    public class FourierProcessing
    {
        public float[]? meineAudioData { get; set; } 
        public int sampleRate { get; set; }

        public List<top5Frequencies> GetIncrements()  //get 0.2 second audio increments w/50% overlap to capture accurate data
        {
            int FFTIncrements = (int)(0.2 * sampleRate);
            float[] InitialFFTWindow = new float[(int)(FFTIncrements * 0.50) + FFTIncrements]; //hann, pad, then convert to complex
            float[] FinalFFTWindow; Complex32[] complexArray; List<top5Frequencies> frequencyList = new List<top5Frequencies>();

            for (int i = 0; i < meineAudioData.Length - (int)(FFTIncrements * 1.5); i += FFTIncrements / 2)
            {
                Array.Copy(meineAudioData, i, InitialFFTWindow, 0, (int)(FFTIncrements * 0.50) + FFTIncrements);
                InitialFFTWindow = ApplyHannWindow(InitialFFTWindow);

                FinalFFTWindow = ZeroPadArray(InitialFFTWindow, FindNearestPowerOf2());

                //Convert to complex for fft need of a complex plane
                complexArray = FinalFFTWindow.Select(x => new Complex32(x, 0)).ToArray();

                Tuple<List<float>, List<float>> freqAndMag = PerformFFT(complexArray);
                top5Frequencies frequency = new top5Frequencies()
                {
                    Frequencies = freqAndMag.Item1,
                    Magnitudes = freqAndMag.Item2
                };
                frequencyList.Add(frequency);
            }
            return frequencyList;
        }

        private float[] ApplyHannWindow(float[] audioIncrement) //smooth out the ends of the signal
        {
            for (int j = 0; j < audioIncrement.Length; j++)
            {   
                float window = (float)(0.5 * (1 - Math.Cos((2 * Math.PI * j) / (audioIncrement.Length - 1))));
                audioIncrement[j] *= window;
            }

            return audioIncrement;
        }

        private int FindNearestPowerOf2() //ensure that the buffer length is a power of 2 to achieve O(logn) time complexity (Cooley-Turkey algorithm needs this!)
        {
            int sampleSize = (int)((0.2 * sampleRate) * 1.5), placeboNum = 2;
            /*  
            if ((int)(Math.Ceiling(Math.Log(sampleSize) / Math.Log(2))) == (int)(Math.Floor(Math.Log(sampleSize) / Math.Log(2))))
            {
                return sampleSize;
            }
            else
            {
                while (placeboNum < sampleSize)
                {
                    placeboNum *= 2;
                }
            } */
            return sampleSize;
        }

        private float[] ZeroPadArray(float[] InitialFFTWin, int newLength) //make array size a power of 2
        {
            float[] newFFTWindow = new float[newLength];
            Array.Copy(InitialFFTWin, 0, newFFTWindow, 0, InitialFFTWin.Length);

            return newFFTWindow;
        }

        public Tuple<List<float>, List<float>> PerformFFT(Complex32[] FFTWindow)
        {
            Fourier.Forward(FFTWindow, FourierOptions.NoScaling);

            float[] magnitudeSpectrum = FFTWindow.Take(FFTWindow.Length / 2).Select(x => x.Magnitude).ToArray(); //Get magnitudes
            List<int> location = new(); List<float> magnitudes = new();
            Tuple<List<int>, List<float>> audioPeaks = new(location, magnitudes);
            List<float> topFrequencies = new List<float>(); List<float> topMagnitudes = new List<float>();
            Tuple<List<float>, List<float>> returnValue = new(topFrequencies, topMagnitudes);

            for (int i = 1; i < magnitudeSpectrum.Length - 1; i++)
            {
                if (magnitudeSpectrum[i] > magnitudeSpectrum[i + 1] && magnitudeSpectrum[i] > magnitudeSpectrum[i - 1] && magnitudeSpectrum[i] > 10 && magnitudeSpectrum[i] <= 28) //extrapolate out overtone shit by finding peaks
                {
                    location.Add(i);
                    magnitudes.Add(magnitudeSpectrum[i]);
                }
            }
            int takeNum = (location.Count < 5) ? location.Count : 5;
            location = location.OrderByDescending(x => x).Take(takeNum).ToList();

            for (int k = 0; k < location.Count; k++)
            {
                topFrequencies.Add(((float)location[k] / (float)FFTWindow.Length) * (float)sampleRate);
                topMagnitudes.Add((float)magnitudes[k]);
            }
            /*
            if (topFrequencies.Count == 0)
            {
                topFrequencies.Add((100 / (float)FFTWindow.Length) * (float)sampleRate);
            } */
            return returnValue;
        }
    }
}
