using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public enum WeatherStage
    {
        Clear,
        Cloudy,
        Overcast
    }
    class WeatherModel
    {
        Random rand = new Random();

        private double[][] qMatrix =
        {
            new[]{-.4, .3,.1},
            new[]{.4,-.8,.4},
            new[]{.1,.4,-.5}
        };

        double[] probability = { 0.33333, 0.33333, 0.33333 };
        static private WeatherStage stage = WeatherStage.Clear;
        private int time = 0;
        private int stageNum { get { return (int)stage; } }

        public int GetTime()
        {
            return time;
        }

        public void DecreaseTime()
        {
            time--;
        }

        public WeatherStage GetCurrentStage()
        {
            return stage;
        }

        public int GetNextStage()
        {
            int thau = (int)Math.Round(Math.Log(rand.NextDouble()) / qMatrix[stageNum][stageNum] * 24);
            time += thau;
            double[] Probability = GetMatrixProbability(stageNum);
            double q = rand.NextDouble();
            for (int i = 0; i <= 3; i++)
            {
                q -= Probability[i];
                if (!(q <= 0)) continue;
                stage = (WeatherStage)i;
                break;
            }

            return stageNum;
        }

        private double[] GetMatrixProbability(int i)
        {
            double[] Prob = new double[qMatrix[i].Length];
            for (int j = 0; j < 3; j++)
                Prob[j] = i == j ? 0 : -(qMatrix[i][j] / qMatrix[i][i]);
            return Prob;
        }

        public (bool, double) GetChiSquare(double[] RealPrpbability, int n)
        {

            int[] massP = new int[3];
            int[] massFreq = new int[3];
            for (int i = 0; i < 3; i++)
            {
                massP[i] = (int)(probability[i] * n);
                massFreq[i] = (int)(RealPrpbability[i] * n);
            }
            double ChiSquareEmperic = 0;

            for (int i = 0; i < 3; i++)
                ChiSquareEmperic += Math.Pow((massFreq[i] - massP[i]), 2) / massP[i];

            return (ChiSquareEmperic < 5.991, ChiSquareEmperic);
        }
    }
}
