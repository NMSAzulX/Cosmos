﻿using Cosmos.Logging.Core;

namespace Cosmos.Logging.RunsOn.NancyFX.Core {
    internal static class HandlerLoggerContainer {
        public static ILogger ErrorHandlerLogger<T>() => StaticServiceResolver.Instance.GetLogger<T>();
    }
}