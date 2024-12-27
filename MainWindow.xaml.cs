using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SemestrControlBelousovFedorov
{
    public partial class MainWindow : Window
    {
        // Вопросы и ответы
        private List<Question> Questions = new List<Question>()
        {
            new Question
            {
                Type = QuestionType.SingleChoice,
                Text = "Что такое инкапсуляция?",
                Choices = new string[] {"Это механизм защиты данных", "Это способ скрыть данные от внешнего мира", "Это способ сделать код более читаемым"},
                CorrectAnswer = "Это способ скрыть данные от внешнего мира"
            },
            new Question
            {
                Type = QuestionType.MultipleChoice,
                Text = "Какие принципы ООП вы знаете?",
                Choices = new string[] {"Инкапсуляция", "Наследование", "Полиморфизм", "Абстракция"},
                CorrectAnswers = new string[] {"Инкапсуляция", "Наследование", "Полиморфизм", "Абстракция"}
            },
            new Question
            {
                Type = QuestionType.SingleChoice,
                Text = "Как называется процесс создания нового класса на основе существующего?",
                Choices = new string[] {"Инкапсуляция", "Наследование", "Полиморфизм", "Абстракция"},
                CorrectAnswer = "Наследование"
            },
            new Question
            {
                Type = QuestionType.MultipleChoice,
                Text = "Что такое полиморфизм?",
                Choices = new string[] {"Способность объекта принимать разные формы", "Механизм наследования классов", "Метод скрытия данных", "Принцип абстракции"},
                CorrectAnswers = new string[] {"Способность объекта принимать разные формы"}
            },
            new Question
            {
                Type = QuestionType.TextInput,
                Text = "Введите определение термина 'абстракция'",
                Answer = "Процесс выделения существенных характеристик объекта, которые отличают его от всех других видов объектов."
            }
        };

        // Баллы за правильные ответы
        private Dictionary<QuestionType, int> PointsPerQuestion = new Dictionary<QuestionType, int>()
        {
            [QuestionType.SingleChoice] = 1,
            [QuestionType.MultipleChoice] = 2,
            [QuestionType.TextInput] = 4
        };

        // Текущий номер вопроса
        private int CurrentQuestionIndex = 0;

        // Общее количество набранных баллов
        private int TotalPoints = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartTesting_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(SurnameEntry.Text))
            {
                MessageBox.Show("Пожалуйста, введите ваше имя и фамилию.");
                return;
            }

            StartPage.Visibility = Visibility.Hidden;
            QuestionPage.Visibility = Visibility.Visible;
            ShowNextQuestion();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CheckCurrentQuestion();
            CurrentQuestionIndex++;

            if (CurrentQuestionIndex < Questions.Count)
            {
                ShowNextQuestion();
            }
            else
            {
                ShowResults();
            }
        }

        private void ShowNextQuestion()
        {
            Question currentQuestion = Questions[CurrentQuestionIndex];
            QuestionLabel.Text = $"Вопрос {CurrentQuestionIndex + 1}: {currentQuestion.Text}";

            switch (currentQuestion.Type)
            {
                case QuestionType.SingleChoice:
                    ShowSingleChoiceQuestion(currentQuestion);
                    break;
                case QuestionType.MultipleChoice:
                    ShowMultipleChoiceQuestion(currentQuestion);
                    break;
                case QuestionType.TextInput:
                    ShowTextInputQuestion(currentQuestion);
                    break;
            }
        }

        private void ShowSingleChoiceQuestion(Question question)
        {
            StackPanel choicesPanel = new StackPanel();
            RadioButton selectedRadioButton = null;

            foreach (string choice in question.Choices)
            {
                RadioButton radioButton = new RadioButton
                {
                    Content = choice,
                    GroupName = "ChoicesGroup",
                    FontSize = 14
                };
                choicesPanel.Children.Add(radioButton);

                if (selectedRadioButton == null)
                {
                    selectedRadioButton = radioButton;
                }
            }

            selectedRadioButton.IsChecked = true;
            QuestionContent.Content = choicesPanel;
        }

        private void ShowMultipleChoiceQuestion(Question question)
        {
            StackPanel choicesPanel = new StackPanel();

            foreach (string choice in question.Choices)
            {
                CheckBox checkBox = new CheckBox
                {
                    Content = choice,
                    FontSize = 14
                };
                choicesPanel.Children.Add(checkBox);
            }

            QuestionContent.Content = choicesPanel;
        }

        private void ShowTextInputQuestion(Question question)
        {
            TextBox answerEntry = new TextBox
            {
                FontSize = 14,
                AcceptsReturn = false,
                MaxLength = 100
            };
            QuestionContent.Content = answerEntry;
        }

        private void CheckCurrentQuestion()
        {
            Question currentQuestion = Questions[CurrentQuestionIndex];

            switch (currentQuestion.Type)
            {
                case QuestionType.SingleChoice:
                    CheckSingleChoiceQuestion(currentQuestion);
                    break;
                case QuestionType.MultipleChoice:
                    CheckMultipleChoiceQuestion(currentQuestion);
                    break;
                case QuestionType.TextInput:
                    CheckTextInputQuestion(currentQuestion);
                    break;
            }
        }

        private void CheckSingleChoiceQuestion(Question question)
        {
            StackPanel choicesPanel = (StackPanel)QuestionContent.Content;
            RadioButton selectedRadioButton = (RadioButton)choicesPanel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);

            if (selectedRadioButton != null && selectedRadioButton.Content.ToString() == question.CorrectAnswer)
            {
                TotalPoints += PointsPerQuestion[question.Type];
            }
            else
            {
                MessageBox.Show($"Неправильный ответ! Правильный ответ: {question.CorrectAnswer}");
            }
        }

        private void CheckMultipleChoiceQuestion(Question question)
        {
            StackPanel choicesPanel = (StackPanel)QuestionContent.Content;
            bool allCorrect = true;

            foreach (CheckBox checkBox in choicesPanel.Children.OfType<CheckBox>())
            {
                if ((checkBox.IsChecked ?? false) != question.CorrectAnswers.Contains(checkBox.Content.ToString()))
                {
                    allCorrect = false;
                    break;
                }
            }

            if (allCorrect)
            {
                TotalPoints += PointsPerQuestion[question.Type];
            }
            else
            {
                MessageBox.Show($"Неправильные ответы! Правильные ответы: {string.Join(", ", question.CorrectAnswers)}");
            }
        }

        private void CheckTextInputQuestion(Question question)
        {
            TextBox answerEntry = (TextBox)QuestionContent.Content;

            if (answerEntry.Text.Trim().ToLower() == question.Answer.ToLower())
            {
                TotalPoints += PointsPerQuestion[question.Type];
            }
            else
            {
                MessageBox.Show($"Неправильный ответ! Правильный ответ: {question.Answer}");
            }
        }

        private void ShowResults()
        {
            QuestionPage.Visibility = Visibility.Hidden;
            ResultPage.Visibility = Visibility.Visible;
            ResultLabel.Text = $"Тест завершён!\nИмя: {NameEntry.Text} {SurnameEntry.Text}\nБаллы: {TotalPoints}/10";
        }
    }
}