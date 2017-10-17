using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        private bool reachedEnd = false;
        //List of all visited Points
        private List<Point> visited = new List<Point>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            //Starting Coordinates
            int x = 0;
            int y = 0;

            robot.ReachedExit += (_, __) => this.reachedEnd = true;

            this.checkDirection(x, y);

            if (this.reachedEnd == false)
            {
                robot.HaltAndCatchFire();
            }
        }

        //Backtracker 
        public void checkDirection(int x, int y)
        {
            if (this.visited.Contains(new Point(x, y)) == false && this.reachedEnd == false)
            {
                this.visited.Add(new Point(x, y));

                if (this.reachedEnd == false && this.robot.TryMove(Direction.Left) == true)
                {
                    this.checkDirection(x - 1, y);
                    if (this.reachedEnd == false)
                    {
                        this.robot.Move(Direction.Right);
                    }
                }

                if (this.reachedEnd == false && this.robot.TryMove(Direction.Right) == true)
                {
                    this.checkDirection(x + 1, y);
                    if (this.reachedEnd == false)
                    {
                        this.robot.Move(Direction.Left);
                    }
                }

                if (this.reachedEnd == false && this.robot.TryMove(Direction.Up) == true)
                {
                    this.checkDirection(x, y - 1);
                    if (this.reachedEnd == false)
                    {
                        this.robot.Move(Direction.Down);
                    }
                }

                if (this.reachedEnd == false && this.robot.TryMove(Direction.Down) == true)
                {
                    this.checkDirection(x, y + 1);
                    if (this.reachedEnd == false)
                    {
                        this.robot.Move(Direction.Up);
                    }
                }
            }
        }
    }
}
