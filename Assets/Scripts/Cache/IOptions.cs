namespace Answerquestions {
    /// <summary>
    /// 用于检索已配置的 TOptions 实例。
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IOptions<out TOptions> where TOptions : class {
        /// <summary>
        /// 获取默认的已配置的 TOptions 实例。
        /// </summary>
        public TOptions Value { get; }
    }
}