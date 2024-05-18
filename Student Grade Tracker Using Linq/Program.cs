using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Student_Grade_Tracker_using_linq
{
	internal class Program
	{
		static List<Student> students = new List<Student>();

		static void Main(string[] args)
		{
			InitializeStudents();

			while (true)
			{
				Console.WriteLine("Enter a number:");
				Console.WriteLine("1 - Add a new student to the system");
				Console.WriteLine("2 - Add grades for a student in different subjects");
				Console.WriteLine("3 - View all students and their grades");
				Console.WriteLine("4 - Calculate and display the average score for each student");
				Console.WriteLine("5 - Search for a student by name or Student ID");
				Console.WriteLine("6 - Exit");

				string key = Console.ReadLine();
				switch (key)
				{
					case "1":
						AddNewStudent();
						break;
					case "2":
						AddGrades();
						break;
					case "3":
						ViewAllStudents();
						break;
					case "4":
						CalculateAverageScore();
						break;
					case "5":
						SearchStudent();
						break;
					case "6":
						return;
					default:
						Console.WriteLine("Invalid option, please try again.");
						break;
				}
			}
		}

		private static void InitializeStudents()
		{
			students.Add(new Student
			{
				Id = 1,
				Name = "John Doe",
				Description = "Student 1",
				Grades = new List<Grade>
				{
					new Grade { Subject = "Math", Score = 90, Date = DateTime.Now },
					new Grade { Subject = "Science", Score = 85, Date = DateTime.Now }
				}
			});
			students.Add(new Student
			{
				Id = 2,
				Name = "Jane Smith",
				Description = "Student 2",
				Grades = new List<Grade>
				{
					new Grade { Subject = "Math", Score = 85, Date = DateTime.Now },
					new Grade { Subject = "Science", Score = 92, Date = DateTime.Now }
				}
			});
			students.Add(new Student
			{
				Id = 3,
				Name = "Alice Johnson",
				Description = "Student 3",
				Grades = new List<Grade>
				{
					new Grade { Subject = "Math", Score = 78, Date = DateTime.Now },
					new Grade { Subject = "Science", Score = 88, Date = DateTime.Now }
				}
			});
		}

		private static void AddNewStudent()
		{
			Console.WriteLine("Enter student name:");
			string name = Console.ReadLine();

			Console.WriteLine("Enter student ID:");
			int id = Convert.ToInt32(Console.ReadLine());

			Console.WriteLine("Enter description:");
			string description = Console.ReadLine();

			var newStudent = new Student { Id = id, Name = name, Description = description };
			students.Add(newStudent);

			Console.WriteLine($"Added new student: {newStudent.Name}");
		}

		private static void AddGrades()
		{
			Console.WriteLine("Enter student ID:");
			int id = Convert.ToInt32(Console.ReadLine());

			var student = students.FirstOrDefault(s => s.Id == id);
			if (student != null)
			{
				Console.WriteLine("Enter subject:");
				string subject = Console.ReadLine();

				Console.WriteLine("Enter score:");
				int score = Convert.ToInt32(Console.ReadLine());

				var grade = new Grade { Subject = subject, Score = score, Date = DateTime.Now };
				student.Grades.Add(grade);

				Console.WriteLine($"Added grade for {student.Name}: {subject} - {score}");
			}
			else
			{
				Console.WriteLine("Student not found.");
			}
		}

		private static void ViewAllStudents()
		{
			foreach (var student in students)
			{
				Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Description: {student.Description}");
				foreach (var grade in student.Grades)
				{
					Console.WriteLine($"  Subject: {grade.Subject}, Score: {grade.Score}, Date: {grade.Date}");
				}
			}
		}

		private static void CalculateAverageScore()
		{
			Console.WriteLine("Enter student name:");
			string studentName = Console.ReadLine();

			var student = students.FirstOrDefault(s => s.Name.Equals(studentName, StringComparison.OrdinalIgnoreCase));
			if (student != null)
			{
				var averageScore = student.Grades.Select(g => g.Score).Average();
				Console.WriteLine($"Average score for {student.Name}: {averageScore}");
			}
			else
			{
				Console.WriteLine("Student not found.");
			}
		}

		private static void SearchStudent()
		{
			Console.WriteLine("Enter student ID:");
			int id = Convert.ToInt32(Console.ReadLine());

			var student = students.FirstOrDefault(s => s.Id == id);
			if (student != null)
			{
				Console.WriteLine($"Found student: {student.Name}");
			}
			else
			{
				Console.WriteLine("Student not found.");
			}
		}
	}

	internal class Student : IEnumerable<Grade>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<Grade> Grades { get; set; } = new List<Grade>();

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			Student otherStudent = (Student)obj;

			return Id == otherStudent.Id &&
				   Name == otherStudent.Name &&
				   Description == otherStudent.Description &&
				   GradesEqual(otherStudent.Grades);
		}

		public void MakeEqual(Student otherStudent)
		{
			Id = otherStudent.Id;
			Name = otherStudent.Name;
			Description = otherStudent.Description;
			Grades = new List<Grade>(otherStudent.Grades);
		}

		private bool GradesEqual(List<Grade> otherGrades)
		{
			if (Grades.Count != otherGrades.Count)
			{
				return false;
			}

			for (int i = 0; i < Grades.Count; i++)
			{
				if (!Grades[i].Equals(otherGrades[i]))
				{
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			int hash = Id.GetHashCode();
			hash = (hash * 7) + Name.GetHashCode();
			hash = (hash * 7) + Description.GetHashCode();
			foreach (var grade in Grades)
			{
				hash = (hash * 7) + grade.GetHashCode();
			}
			return hash;
		}

		public IEnumerator<Grade> GetEnumerator()
		{
			return Grades.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	internal class Grade
	{
		public string Subject { get; set; }
		public DateTime Date { get; set; }
		public int Score { get; set; }

		public Grade()
		{
			Subject = "Default Subject";
			Score = 0;  // Changed from 500 to 0 as it seems more realistic for a default score
			Date = DateTime.Now;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			Grade otherGrade = (Grade)obj;

			return Subject == otherGrade.Subject &&
				   Score == otherGrade.Score &&
				   Date == otherGrade.Date;
		}

		public override int GetHashCode()
		{
			int hash = Subject.GetHashCode();
			hash = (hash * 7) + Score.GetHashCode();
			hash = (hash * 7) + Date.GetHashCode();
			return hash;
		}
	}
}
