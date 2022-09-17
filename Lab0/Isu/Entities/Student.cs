using System;
using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;
public class Student
{
        public Student(string? name, Group? groupNumber)
        {
                if (string.IsNullOrEmpty(name))
                        throw new StudentException("Student name does not exist");
                FullName = name;
                Id = Id++;
                Group = groupNumber ?? throw new GroupException("This group does not exist");
        }

        public int Id { get; } = 1;
        public string FullName { get; }
        public Group Group { get; private set; }
        public void ChangeGroup(Group newGroup)
        {
                if (newGroup is null)
                        throw new GroupException("There is no group to transfer the student to");
                Group oldGroup = Group;
                newGroup.AddStudent(this);
                oldGroup.RemoveStudent(this);
                Group = newGroup;
        }
}