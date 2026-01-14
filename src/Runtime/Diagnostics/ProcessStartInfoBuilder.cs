// -----------------------------------------------------------------------
// <copyright file="ProcessStartInfoBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Diagnostics;

using System.Diagnostics;

/// <summary>
/// The <see cref="ProcessStartInfo"/> builder.
/// </summary>
public class ProcessStartInfoBuilder
{
    private readonly string fileName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessStartInfoBuilder"/> class.
    /// </summary>
    /// <param name="fileName">The file name.</param>
    public ProcessStartInfoBuilder(string fileName)
        : this(new ProcessStartInfo(fileName))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessStartInfoBuilder"/> class.
    /// </summary>
    /// <param name="startInfo">The start info.</param>
    public ProcessStartInfoBuilder(ProcessStartInfo startInfo)
    {
        this.fileName = startInfo.FileName;
        this.CreateNoWindow = startInfo.CreateNoWindow;
        this.UserName = startInfo.UserName;
        this.UseShellExecute = startInfo.UseShellExecute;
        this.WorkingDirectory = startInfo.WorkingDirectory;
#if NET5_0_OR_GREATER
        if (OperatingSystem.IsWindows())
        {
            this.LoadUserProfile = startInfo.LoadUserProfile;
            this.Password = startInfo.Password;
            this.PasswordInClearText = startInfo.PasswordInClearText;
            this.Domain = startInfo.Domain;
        }
#else
        this.LoadUserProfile = startInfo.LoadUserProfile;
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER
        this.Password = startInfo.Password;
#endif
#if NETSTANDARD1_4_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET461_OR_GREATER
        this.PasswordInClearText = startInfo.PasswordInClearText;
#endif
        this.Domain = startInfo.Domain;
#endif
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER
        this.ErrorDialog = startInfo.ErrorDialog;
        this.ErrorDialogParentHandle = startInfo.ErrorDialogParentHandle;
        this.Verb = startInfo.Verb;
        this.Verbs = [.. startInfo.Verbs];
        this.WindowStyle = startInfo.WindowStyle;
#endif
#if NETSTANDARD1_4_OR_GREATER || NET46_OR_GREATER || NETCOREAPP1_0_OR_GREATER
        this.Environment = new Dictionary<string, string?>(startInfo.Environment, StringComparer.OrdinalIgnoreCase);
#else
        foreach (string key in startInfo.EnvironmentVariables.Keys)
        {
            this.EnvironmentVariables.Add(key, startInfo.EnvironmentVariables[key]);
        }
#endif
    }

    /// <inheritdoc cref="ProcessStartInfo.CreateNoWindow"/>
    public bool CreateNoWindow { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.UserName"/>
    public string UserName { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.UseShellExecute"/>
    public bool UseShellExecute { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.WorkingDirectory"/>
    public string WorkingDirectory { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.Domain"/>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public string? Domain { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.LoadUserProfile"/>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public bool LoadUserProfile { get; set; }

#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER
    /// <inheritdoc cref="ProcessStartInfo.ErrorDialog"/>
    public bool ErrorDialog { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.ErrorDialogParentHandle"/>
    public IntPtr ErrorDialogParentHandle { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.Password"/>
    [CLSCompliant(false)]
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public System.Security.SecureString? Password { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.Verb"/>
    public string Verb { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.Verbs"/>
    public IReadOnlyCollection<string> Verbs { get; set; }

    /// <inheritdoc cref="ProcessStartInfo.WindowStyle"/>
    public ProcessWindowStyle WindowStyle { get; set; }
#endif

#if NETSTANDARD1_4_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET461_OR_GREATER
    /// <inheritdoc cref="ProcessStartInfo.PasswordInClearText"/>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public string? PasswordInClearText { get; set; }
#endif

#if NETSTANDARD1_4_OR_GREATER || NET46_OR_GREATER || NETCOREAPP1_0_OR_GREATER
    /// <inheritdoc cref="ProcessStartInfo.Environment"/>
    public IDictionary<string, string?> Environment { get; }
#else
    /// <inheritdoc cref="ProcessStartInfo.EnvironmentVariables"/>
    public System.Collections.Specialized.StringDictionary EnvironmentVariables { get; } = [];
#endif

    /// <summary>
    /// Gets the <see cref="ProcessStartInfo"/> built by this instance.
    /// </summary>
    public ProcessStartInfo ProcessStartInfo
    {
        get
        {
            var processStartInfo = new ProcessStartInfo(this.fileName)
            {
                CreateNoWindow = this.CreateNoWindow,
                UserName = this.UserName,
                UseShellExecute = this.UseShellExecute,
                WorkingDirectory = this.WorkingDirectory,
            };
#if NET5_0_OR_GREATER
            if (OperatingSystem.IsWindows())
            {
                processStartInfo.Domain = this.Domain!;
                processStartInfo.LoadUserProfile = this.LoadUserProfile;
                processStartInfo.Password = this.Password;
                processStartInfo.PasswordInClearText = this.PasswordInClearText;
            }
#else
            processStartInfo.Domain = this.Domain;
            processStartInfo.LoadUserProfile = this.LoadUserProfile;
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER
            processStartInfo.Password = this.Password;
#endif
#if NETSTANDARD1_4_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET461_OR_GREATER
            processStartInfo.PasswordInClearText = this.PasswordInClearText;
#endif
#endif

#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER
            processStartInfo.ErrorDialog = this.ErrorDialog;
            processStartInfo.ErrorDialogParentHandle = this.ErrorDialogParentHandle;
            processStartInfo.Verb = this.Verb;
            processStartInfo.WindowStyle = this.WindowStyle;
#endif

#if NETSTANDARD1_4_OR_GREATER || NET46_OR_GREATER || NETCOREAPP1_0_OR_GREATER
            foreach (var kvp in this.Environment)
            {
                processStartInfo.Environment.Add(kvp.Key, kvp.Value);
            }
#else
            foreach (string key in this.EnvironmentVariables.Keys)
            {
                processStartInfo.EnvironmentVariables.Add(key, this.EnvironmentVariables[key]);
            }
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            foreach (var argument in this.GetArguments())
            {
                processStartInfo.ArgumentList.Add(argument);
            }
#else
            processStartInfo.Arguments = string.Join(" ", this.GetArguments());
#endif

            return processStartInfo;
        }
    }

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    /// <param name="processStartInfo">The process start info to get the arguments from.</param>
    /// <returns>The arguments.</returns>
    public static string GetArguments(ProcessStartInfo processStartInfo) =>
        processStartInfo switch
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            { ArgumentList: { Count: > 0 } argumentList } => string.Join(' ', argumentList),
#endif
            { Arguments: var arguments } => arguments,
        };

    /// <summary>
    /// Gets the argument list.
    /// </summary>
    /// <param name="processStartInfo">The process start info to get the arguments from.</param>
    /// <returns>The argument list.</returns>
    public static IReadOnlyList<string> GetArgumentList(ProcessStartInfo processStartInfo) =>
        processStartInfo switch
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            { ArgumentList: { Count: > 0 } argumentList } => argumentList,
#endif
            { Arguments: { } arguments } => arguments.Split(' '),
            _ => [],
        };

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    /// <returns>The arguments.</returns>
    protected virtual IEnumerable<string> GetArguments()
    {
        yield break;
    }
}