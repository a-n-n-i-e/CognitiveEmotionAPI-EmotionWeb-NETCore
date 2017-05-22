using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// 追加
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

namespace EmotionWeb.Controllers
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
            // 取得可否を設定
            var res = "";
            // フォームからの入力(URL)を取得
            ViewData["Url"] = url;

            // Emotion API の Subscription Key をセット
            // お持ちの Subscription Key を YOUR_SUBSCRIPTION_KEY 部分にコピーしてください
            const string eApiKey = "YOUR_SUBSCRIPTION_KEY";

            // Emotion API を Call する EmotionServiceClient を生成
            var eClient = new EmotionServiceClient(eApiKey);
            // 結果を取得する配列を定義
            Emotion[] eResult = null;

            // Emotion API を呼び出して結果を取得
            try
            {
                eResult = await eClient.RecognizeAsync(url);
            }
            catch (Exception e)
            {
                eResult = null;
            }

            // 呼び出し結果を元に表示内容を作成
            if (eResult != null)
            {
                //表情スコアを取得
                try
                {
                    res = "結果:";
                    var eScores = eResult[0].Scores;

                    // 検出された表情スコアをセット
                    ViewData["Anger"] = Math.Round(eScores.Anger, 6).ToString("0.000000");
                    ViewData["Contempt"] = Math.Round(eScores.Contempt, 6).ToString("0.000000");
                    ViewData["Disgust"] = Math.Round(eScores.Disgust, 6).ToString("0.000000");
                    ViewData["Fear"] = Math.Round(eScores.Fear, 6).ToString("0.000000");
                    ViewData["Happiness"] = Math.Round(eScores.Happiness, 6).ToString("0.000000");
                    ViewData["Neutral"] = Math.Round(eScores.Neutral, 6).ToString("0.000000");
                    ViewData["Sadness"] = Math.Round(eScores.Sadness, 6).ToString("0.000000");
                    ViewData["Surprise"] = Math.Round(eScores.Surprise, 6).ToString("0.000000");
                }
                catch (Exception e)
                {
                    res = "検出できませんでした";
                }
            }
            else
            {
                res = "API の呼び出しに失敗しました";
            }

            // 結果を表示
            ViewData["Result"] = res;
            return View();

        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
