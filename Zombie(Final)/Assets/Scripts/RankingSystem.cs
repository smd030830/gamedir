using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 게임 종료 점수를 PlayerPrefs에 저장하고 상위 랭킹을 제공한다
public static class RankingSystem {
    private const string RankingKey = "ZombieRankingScores";
    private const int MaxRankCount = 5;

    public static void RegisterScore(int score) {
        List<int> scores = LoadScores();
        scores.Add(score);
        scores.Sort((left, right) => right.CompareTo(left));

        if (scores.Count > MaxRankCount)
        {
            scores.RemoveRange(MaxRankCount, scores.Count - MaxRankCount);
        }

        PlayerPrefs.SetString(RankingKey, string.Join(",", scores));
        PlayerPrefs.Save();
    }

    public static List<int> LoadScores() {
        List<int> scores = new List<int>();
        string rawScores = PlayerPrefs.GetString(RankingKey, string.Empty);

        if (string.IsNullOrEmpty(rawScores))
        {
            return scores;
        }

        string[] splitScores = rawScores.Split(',');
        for (int i = 0; i < splitScores.Length; i++)
        {
            if (int.TryParse(splitScores[i], out int score))
            {
                scores.Add(score);
            }
        }

        scores.Sort((left, right) => right.CompareTo(left));
        return scores;
    }

    public static string FormatRanking() {
        List<int> scores = LoadScores();

        if (scores.Count <= 0)
        {
            return "No ranking yet";
        }

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < scores.Count; i++)
        {
            builder.Append(i + 1);
            builder.Append(". ");
            builder.Append(scores[i]);

            if (i < scores.Count - 1)
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }
}
