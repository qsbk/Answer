using System;

namespace Answerquestions {     
    public class MemoryCacheOptions : IOptions<MemoryCacheOptions> {
        private long? _sizeLimit;
        private double _compactionPercentage = 0.05;
        /// <summary>
        /// 扫描频率
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1);
        /// <summary>
        /// 获取或设置缓存的最大大小
        /// </summary>
        public long? SizeLimit {
            get => _sizeLimit;
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be non-negative.");
                }

                _sizeLimit = value;
            }
        }
        /// <summary>
        /// 获取或设置当缓存超过最大大小时，压缩的数量
        /// </summary>
        public double CompactionPercentage {
            get => _compactionPercentage;
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be between 0 and 1 inclusive.");
                }

                _compactionPercentage = value;
            }
        }
        MemoryCacheOptions IOptions<MemoryCacheOptions>.Value {
            get { return this; }
        }
    }
}