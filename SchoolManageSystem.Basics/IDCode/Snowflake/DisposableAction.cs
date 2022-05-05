using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.IDCode.Snowflake
{
    /// <summary>
    /// 可能性
    /// </summary>
    public class DisposableAction : IDisposable
    {
        readonly Action _action;

        public DisposableAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
