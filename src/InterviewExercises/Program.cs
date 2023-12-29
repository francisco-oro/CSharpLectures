using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewExercises
{

    public class Exercise
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(NotHungryCats("F O~ O~ ~O O~"));
            Console.ReadKey();
        }
        public static int NotHungryCats(string kitchen)
        {
            int count = 0;
            try
            {
                int UserPosition = kitchen.IndexOf('F');
                kitchen = kitchen.Replace(" ", "");
                for (int i = 0; i < UserPosition; i += 2)
                {
                    if (GetDirection(kitchen.Substring(i, 2)).Equals(direction.left))
                        count++;
                }
                for (int i = UserPosition + 1; i < kitchen.Length; i += 2)
                {
                    if (GetDirection(kitchen.Substring(i, 2)).Equals(direction.right))
                        count++;
                }
            } catch (Exception ex) {  Console.WriteLine(ex); }
            return count;
        }

        private static direction GetDirection(string cat)
        {
            if (cat.Equals("~O"))
                return direction.right;
            if(cat.Equals("O~"))
                return direction.left;
            return direction.up;        
        }
        private enum direction
        {
            left,
            right,
            up
        }

    }
}
