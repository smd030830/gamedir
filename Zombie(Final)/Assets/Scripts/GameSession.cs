// 씬 사이에서 한 판의 결과를 전달하는 간단한 세션 저장소
public static class GameSession {
    public static int lastScore { get; private set; }

    public static void FinishRun(int score) {
        lastScore = score;
        RankingSystem.RegisterScore(score);
    }
}
