using System;
using Isu.Models;

namespace Isu.Entities;
public class Student
{
        public Student(string name, Group groupNumber)
        {
                FullName = name;
                Id = Id++;
                Group = groupNumber;
        }

        public int Id { get; private set; } = 1;
        public string FullName { get; private set; }
        public Group Group { get; set; }
}