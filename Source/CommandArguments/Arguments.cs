﻿using System.Collections.Generic;
using System.Linq;

namespace CommandArguments
{
    /// <summary>
    /// All the arguments.
    /// </summary>
    public class Arguments : List<IArgument>
    {
        private readonly IEnumerable<IArgument> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments" /> class.
        /// </summary>
        /// <param name="source">The source arguments.</param>
        /// <param name="arguments">Arguments to process.</param>
        public Arguments(string[] source, IEnumerable<IArgument> arguments) : this()
        {
            Source = source;
            this.arguments = arguments;
        }

        /// <summary>
        /// Initializes a new instance of the<see cref="Arguments" /> class.
        /// </summary>
        /// <param name="source">The source arguments.</param>
        public Arguments(string[] source) : this()
        {
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the<see cref="Arguments" /> class.
        /// </summary>
        public Arguments()
        {
        }

        /// <summary>
        /// The source arguments.
        /// </summary>
        public string[] Source { get; set; }

        /// <summary>
        /// The strings separating the arguments.
        /// </summary>
        public string[] ArgumentSeparators { get; set; } = {"-", "--", "/", "//"};

        /// <summary>
        /// The character that seperates the flag from the parameter.
        /// </summary>
        public char ParameterSeparator { get; set; } = ':';

        /// <summary>
        /// Executes the action on each of the arguments encountered.
        /// </summary>
        /// <returns></returns>
        public Arguments Process()
        {
            AddRange(arguments);

            foreach (var sourceArgument in Source)
            {
                var command = sourceArgument.Trim().TrimStart('/', '-');
                var flag = command.Split(ParameterSeparator)[0];
                var parameter = command.Split(ParameterSeparator).Length > 1 ? command.Split(ParameterSeparator)[1] : string.Empty;
                var argument = this.FirstOrDefault(f => f.Flags.Contains(flag));
                if (argument != null)
                {
                    argument.Action.Invoke(parameter);
                    if (argument.TerminateAfterExecution) break;
                }
            }
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments" /> class with the specified source arguments.
        /// </summary>
        /// <param name="source">The source arguments.</param>
        /// <returns>The arguments with the specified source arguments.</returns>
        public static Arguments NewArguments(string[] source)
        {
            return new Arguments(source);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Arguments" /> class with the specified source arguments and injected arguments.
        /// </summary>
        /// <param name="source">The source arguments.</param>
        /// <param name="arguments">Arguments to inject.</param>
        /// <returns>The arguments with the specified source arguments and injected arguments.</returns>
        public static Arguments NewArguments(string[] source, IEnumerable<IArgument> arguments)
        {
            return new Arguments(source, arguments);
        }
    }
}