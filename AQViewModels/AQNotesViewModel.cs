﻿using CommunityToolkit.Mvvm.Input;
using AQAppClase.AQViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AQAppClase.AQViewModels
{
    internal class NotesViewModel : IQueryAttributable
    {
        public ObservableCollection<AQViewModels.AQNoteViewModel> AllNotes { get; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }

        public NotesViewModel()
        {
            AllNotes = new ObservableCollection<AQViewModels.AQNoteViewModel>(AQModels.AQNote.LoadAll().Select(n => new AQNoteViewModel(n)));
            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<AQViewModels.AQNoteViewModel>(SelectNoteAsync);
        }

        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(AQViews.AQNotePage));
        }

        private async Task SelectNoteAsync(AQViewModels.AQNoteViewModel note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(AQViews.AQNotePage)}?load={note.Identifier}");
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                AQNoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note exists, delete it
                if (matchedNote != null)
                    AllNotes.Remove(matchedNote);
            }
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                AQNoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note is found, update it
                if (matchedNote != null)
                {
                    matchedNote.Reload();
                    AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                }
                // If note isn't found, it's new; add it.
                else
                    AllNotes.Insert(0, new AQNoteViewModel(AQModels.AQNote.Load(noteId)));
            }
        }
    }

}