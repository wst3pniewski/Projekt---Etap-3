using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action? execute, Func<bool>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return this._canExecute();
        }

        public virtual void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged;

        internal void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class BallInstances : INotifyPropertyChanged
    {
        private Vector2 _position;

        public int Radius { get; }

        public float X
        {
            get
            {
                return _position.X;
            }
            set
            {
                _position.X = value;
                OnPropertyChanged();
            }
        }
        public float Y
        {
            get { return _position.Y; }
            set
            {
                _position.Y = value;
                OnPropertyChanged();
            }
        }

        public BallInstances(int radius)
        {
            X = 0;
            Y = 0;
            Radius = radius;
        }
        public BallInstances(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
            Radius = 10;
        }

        public void Move(Vector2 direction)
        {
            X = direction.X;
            Y = direction.Y;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Wzorzec projektowy Observer, inne klasy mogą zasubskrybować się na zmiany w tej klasie, w setterach OnPropertyChanged() wywołuje się event PropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection() {}

        public AsyncObservableCollection(IEnumerable<T> list) : base(list) {}

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaiseCollectionChanged(e);
            }
            else
            {
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaisePropertyChanged(e);
            }
            else
            {
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        private void RaiseCollectionChanged(object e)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)e);
        }

        private void RaisePropertyChanged(object e)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs)e);
        }
    }

    // pośredniczy pomiędzy modelem a view
    public class ViewModel : INotifyPropertyChanged
    {
        #region privateProperties

        private ModelClass _model;

        #endregion

        #region publicProperties

        public AsyncObservableCollection<BallInstances> Balls { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int NumberOfBalls
        {
            get { return _model.GetBallsNumber(); }
            set
            {
                if (value >= 0)
                {
                    _model.SetBallsNumber(value);
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region iCommand

        public ICommand StartButton { get; }
        public ICommand StopButton { get; }
        public ICommand AddBallButton { get; }
        public ICommand RemoveBallButton { get; }

        #endregion

        #region constructors

        public ViewModel()
        {
            Balls = new AsyncObservableCollection<BallInstances>();
            _model = new ModelClass();
            NumberOfBalls = 4;

            AddBallButton = new RelayCommand(() =>
            {
                NumberOfBalls += 1;
            });

            RemoveBallButton = new RelayCommand(() =>
            {
                NumberOfBalls -= 1;
            });

            StartButton = new RelayCommand(() =>
            {
                if (Balls.Count == 0)
                {
                    _model.SetBallsNumber(NumberOfBalls);
                    _model.AddBallsToLogicLayer(NumberOfBalls);
                    for (var i = 0; i < NumberOfBalls; i++)
                    {
                        Balls.Add(new BallInstances(_model.GetBallRadius(i)));
                    }

                    _model.OnBallPositionChanged += (sender, args) =>
                    {
                        if (Balls.Count > 0)
                        {
                            Balls[args.Id].Move(args.Position);
                        }
                    };

                    _model.StartProgram();
                }
            });

            StopButton = new RelayCommand(() =>
            {
                _model.StopProgram();
                _model.SetBallsNumber(NumberOfBalls);
                Balls.Clear();
            });


        }

        #endregion



        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}