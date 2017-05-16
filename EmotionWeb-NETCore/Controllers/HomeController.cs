using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//追加
using Microsoft.AspNetCore.Http;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EmotionWeb_NETCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Result(string url)
        {
            var responseStatus = "";
            ViewData["Url"] = url;

            //Emotion API の Subscription Key をセット
            //お持ちの Subscription Key を YOUR_SUBSCRIPTION_KEY 部分にコピーしてください
            const string emotionApiKey = "35d9fbb111674c20b0fccd348cff23b3";

            // Emotion API を Call する EmotionServiceClient を生成
            var emotionServiceClient = new EmotionServiceClient(emotionApiKey);
            Emotion[] emotionResult = null;

            try
            {
                emotionResult = await emotionServiceClient.RecognizeAsync(url);
            }
            catch (Exception e)
            {
                emotionResult = null;
            }

            //Call 結果を元に 返答を作成
            if (emotionResult != null)
            {
                //表情スコアを取得
                try
                {
                    responseStatus = "結果:";
                    var emotionScores = emotionResult[0].Scores;

                    // 検出された表情スコアをセット
                    ViewData["Anger"] = Math.Round(emotionScores.Anger, 6).ToString("0.000000");
                    ViewData["Contempt"] = Math.Round(emotionScores.Contempt, 6).ToString("0.000000");
                    ViewData["Disgust"] = Math.Round(emotionScores.Disgust, 6).ToString("0.000000");
                    ViewData["Fear"] = Math.Round(emotionScores.Fear, 6).ToString("0.000000");
                    ViewData["Happiness"] = Math.Round(emotionScores.Happiness, 6).ToString("0.000000");
                    ViewData["Neutral"] = Math.Round(emotionScores.Neutral, 6).ToString("0.000000");
                    ViewData["Sadness"] = Math.Round(emotionScores.Sadness, 6).ToString("0.000000");
                    ViewData["Surprise"] = Math.Round(emotionScores.Surprise, 6).ToString("0.000000");
                }

                catch (Exception e)
                {
                    emotionResult = null;
                    responseStatus = "検出できませんでした";
                }

            }
            else
            {
                responseStatus = "検出できませんでした";
            }

            ViewData["Result"] = responseStatus;
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
