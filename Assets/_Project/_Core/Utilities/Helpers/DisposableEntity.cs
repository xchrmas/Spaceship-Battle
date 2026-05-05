using System;
using System.Collections.Generic;

namespace SpaceshipBattle.Helpers
{
    public class DisposableEntity : IDisposable
    {
        readonly List<IDisposable> _disposables = new();

        public void AddDisposable(IDisposable item) => _disposables.Add(item);

        public void Dispose()
        {
            foreach (var d in _disposables) d.Dispose();
            _disposables.Clear();
        }

        ~DisposableEntity() => Dispose();
    }

    public static class DisposableExtensions
    {
        public static T AddTo<T>(this T disposable, DisposableEntity entity)
            where T : IDisposable
        {
            entity.AddDisposable(disposable);
            return disposable;
        }
    }
}