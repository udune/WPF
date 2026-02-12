using System.Windows;
using ch34_MVVM.Models;
using ch34_MVVM.Commands;

namespace ch34_MVVM.ViewModels
{
    public class PersonViewModel
    {
        public PersonCommand PersonCommand { get; set; }
        public List<PersonModel> PersonList { get; set; }
        public PersonViewModel()
        {
            PersonList = new List<PersonModel>
            {
                new PersonModel {Name = "홍길동", Age = 100},
                new PersonModel {Name = "임꺽정", Age = 90},
                new PersonModel {Name = "타요", Age = 10},
                new PersonModel {Name = "뽀로로", Age = 12},
                new PersonModel {Name = "뽈리", Age = 7}
            };
            PersonCommand = new PersonCommand(Message, CheckMessage);
        }

        private void Message(string? txt)
        {
            MessageBox.Show(txt);
        }

        private bool CheckMessage(string? txt)
        {
            if (txt?.Length > 0) return true;
            return false;
        }
    }
}
