﻿using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {
        private List<ToDo> _toDoList;
        public List<ToDo> ToDos
        {
            get
            {
                return _toDoList.Take(100).ToList();
            }

            private set
            {
                if (value != _toDoList)
                {
                    _toDoList = value;
                }
            }
        }

        private ToDoServiceProxy()
        {
            ToDos = new List<ToDo>
            {
            };
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ToDoServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToDoServiceProxy();
                }

                return instance;
            }
        }
        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            if (toDo != null && toDo.Id == 0)
            {
                toDo.Id = nextKey;
                _toDoList.Add(toDo);
            }
            if (toDo?.Project != null)
                ProjectServiceProxy.Current.AddOrUpdate(toDo.Project);
            return toDo;
        }

        public void DisplayToDos(bool isShowCompleted = false)
        {
            if (isShowCompleted)
            {
                ToDos.ForEach(Console.WriteLine);
            }
            else
            {
                ToDos.Where(t => (t != null) && !(t?.IsCompleted ?? false))
                                .ToList()
                                .ForEach(Console.WriteLine);
            }
        }

        public ToDo? GetById(int id)
        {
            return ToDos.FirstOrDefault(t => t.Id == id);
        }

        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo == null)
            {
                return;
            }
            if (toDo.Project != null)
            {
                toDo.Project.ToDos.Remove(toDo);
                ProjectServiceProxy.Current.AddOrUpdate(toDo.Project);
            }
            _toDoList.Remove(toDo);
        }

    }
}