namespace AdvancedTraining.Lesson49;

public class RobotRoomCleaner //Problem_0489
{
    private static readonly int[][] Ds =
    [
        [-1, 0],
        [0, 1],
        [1, 0],
        [0, -1]
    ];

    // 提交下面的内容
    private static void CleanRoom(IRobot robot)
    {
        Clean(robot, 0, 0, 0, new HashSet<string>());
    }

    // 机器人robot，
    // 当前来到的位置(x,y)，且之前没来过
    // 机器人脸冲什么方向d，0 1 2 3
    // visited里记录了机器人走过哪些位置
    // 函数的功能：不要重复走visited里面的位置，把剩下的位置，都打扫干净！
    //           而且要回去！
    private static void Clean(IRobot robot, int x, int y, int d, HashSet<string> visited)
    {
        robot.Clean();
        visited.Add(x + "_" + y);
        for (var i = 0; i < 4; i++)
        {
            // d = 0 :  0 1 2 3
            // d = 1 :  1 2 3 0
            // d = 2 :  2 3 0 1
            // d = 3 :  3 0 1 2
            // 下一步的方向！
            var nd = (i + d) % 4;
            // 当下一步的方向定了！下一步的位置在哪？(nx, ny)
            var nx = Ds[nd][0] + x;
            var ny = Ds[nd][1] + y;
            if (!visited.Contains(nx + "_" + ny) && robot.move()) Clean(robot, nx, ny, nd, visited);
            robot.turnRight();
        }

        // 负责回去：之前的位置，怎么到你的！你要回去，而且方向和到你之前，要一致！
        robot.turnRight();
        robot.turnRight();
        robot.move();
        robot.turnRight();
        robot.turnRight();
    }

    // 不要提交这个接口的内容
    private interface IRobot
    {
        bool move();

        void turnLeft();

        void turnRight();

        void Clean();
    }
}