using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace ArtC.Utils {
    public class StateMachine<T> : IDisposable where T : struct, IComparable, IConvertible, IFormattable {
        public GameObject Target;
        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }

        protected Subject<(T previous, T next)> _onStateChange = new();
        public Observable<(T previous, T next)> OnStateChange => _onStateChange;

        public StateMachine(GameObject target, T initialState) {
            Target = target;
            CurrentState = initialState;
        }

        public virtual void ChangeState(T newState) {
            if (EqualityComparer<T>.Default.Equals(newState, CurrentState)) {
                return;
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            _onStateChange.OnNext((PreviousState, CurrentState));
        }

        public virtual void RestorePreviousState() {
            (CurrentState, PreviousState) = (PreviousState, CurrentState);

            _onStateChange.OnNext((PreviousState, CurrentState));
        }

        public void Dispose() {
            _onStateChange.Dispose();
        }
    }
}
