﻿using System;
using System.Collections.Generic;
using Cosmos.Logging.Configurations;
using Cosmos.Logging.Core;
using Cosmos.Logging.Events;

namespace Cosmos.Logging.Sinks.Log4Net {
    public class Log4NetSinkOptions : ILoggingSinkOptions<Log4NetSinkOptions>, ILoggingSinkOptions {
        public string Key => Internals.Constants.SinkKey;

        #region Append log minimum level

        internal readonly Dictionary<string, LogEventLevel> InternalNavigatorLogEventLevels = new Dictionary<string, LogEventLevel>();

        internal LogEventLevel? MinimumLevel { get; set; }
        
        public Log4NetSinkOptions UseMinimumLevelForType<T>(LogEventLevel level) => UseMinimumLevelForType(typeof(T), level);

        public Log4NetSinkOptions UseMinimumLevelForType(Type type, LogEventLevel level) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var typeName = TypeNameHelper.GetTypeDisplayName(type);
            if (InternalNavigatorLogEventLevels.ContainsKey(typeName)) {
                InternalNavigatorLogEventLevels[typeName] = level;
            } else {
                InternalNavigatorLogEventLevels.Add(typeName, level);
            }

            return this;
        }
        
        public Log4NetSinkOptions UseMinimumLevelForCategoryName<T>(LogEventLevel level) => UseMinimumLevelForCategoryName(typeof(T), level);

        public Log4NetSinkOptions UseMinimumLevelForCategoryName(Type type, LogEventLevel level) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var @namespace = type.Namespace;
            return UseMinimumLevelForCategoryName(@namespace, level);
        }

        public Log4NetSinkOptions UseMinimumLevelForCategoryName(string categoryName, LogEventLevel level) {
            if (string.IsNullOrWhiteSpace(categoryName)) throw new ArgumentNullException(nameof(categoryName));
            categoryName = $"{categoryName}.*";
            if (InternalNavigatorLogEventLevels.ContainsKey(categoryName)) {
                InternalNavigatorLogEventLevels[categoryName] = level;
            } else {
                InternalNavigatorLogEventLevels.Add(categoryName, level);
            }

            return this;
        }

        public Log4NetSinkOptions UseMinimumLevel(LogEventLevel? level) {
            MinimumLevel = level;
            return this;
        }

        #endregion

        #region Append log level alias

        internal readonly Dictionary<string, LogEventLevel> InternalAliases = new Dictionary<string, LogEventLevel>();

        public Log4NetSinkOptions UseAlias(string alias, LogEventLevel level) {
            if (string.IsNullOrWhiteSpace(alias)) return this;
            if (InternalAliases.ContainsKey(alias)) {
                InternalAliases[alias] = level;
            } else {
                InternalAliases.Add(alias, level);
            }

            return this;
        }

        #endregion
        
        #region Append configuration file path

        public Log4NetSinkOptions UseDefaultOriginConfigFilePath() {
            OriginConfigFilePath = "log4net.config";
            WatchOriginConfigFile = false;
            return this;
        }

        public Log4NetSinkOptions UseDefaultOriginConfigFilePathAndWatch() {
            OriginConfigFilePath = "log4net.config";
            WatchOriginConfigFile = true;
            return this;
        }

        #endregion


        public string OriginConfigFilePath { get; set; }
        public bool WatchOriginConfigFile { get; set; } 
    }
}