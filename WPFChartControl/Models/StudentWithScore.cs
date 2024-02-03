using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChartControl.Models
{
    public class StudentWithScore
    {
        public Student Student { get; set; } = default!;
        public TestScore Score { get; set; } = default!;

        private static IEnumerable<StudentWithScore>? _joinedData;

        public static IEnumerable<StudentWithScore> GetSeedDatas()
        {
            if (_joinedData != null)
                return _joinedData;

            List<Student> students = new()
            {
                new Student {Id = 1, Name = "심유나"},
                new Student {Id = 2, Name = "홍길동"},
                new Student {Id = 3, Name = "유관순"},
                new Student {Id = 4, Name = "강감찬"},
            };

            Random random = new Random();
            List<TestScore> testScores = new List<TestScore>();

            foreach (Student student in students)
            {
                for (int i = 1; i <= 5; i++)
                {
                    DateTime date = new DateTime(2023, i, 1);
                    testScores.Add(new TestScore
                    {
                        Id = i,
                        Date = date,
                        KorScore = random.Next(0, 101),
                        MathScore = random.Next(0, 101),
                        EngScore = random.Next(0, 101),
                        StudentId = student.Id,
                    });
                }
            }

            _joinedData = from student in students
                          join score in testScores on student.Id equals score.StudentId
                          select new StudentWithScore { Student = student, Score = score };

            return _joinedData;
        }
    }
}
