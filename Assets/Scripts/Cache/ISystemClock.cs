using System;

namespace Answerquestions {
    public interface ISystemClock {
        public DateTimeOffset UtcNow { get; }
    }
}