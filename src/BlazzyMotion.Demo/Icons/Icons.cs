using Microsoft.AspNetCore.Components;

namespace BlazzyMotion.Demo.Icons;

/// <summary>
/// Static class containing all SVG icons used in the Demo application.
/// </summary>
public static class Icons
{
    #region Company Logos

    public static MarkupString Apple => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M18.71 19.5c-.83 1.24-1.71 2.45-3.05 2.47-1.34.03-1.77-.79-3.29-.79-1.53 0-2 .77-3.27.82-1.31.05-2.3-1.32-3.14-2.53C4.25 17 2.94 12.45 4.7 9.39c.87-1.52 2.43-2.48 4.12-2.51 1.28-.02 2.5.87 3.29.87.78 0 2.26-1.07 3.81-.91.65.03 2.47.26 3.64 1.98-.09.06-2.17 1.28-2.15 3.81.03 3.02 2.65 4.03 2.68 4.04-.03.07-.42 1.44-1.38 2.83M13 3.5c.73-.83 1.94-1.46 2.94-1.5.13 1.17-.34 2.35-1.04 3.19-.69.85-1.83 1.51-2.95 1.42-.15-1.15.41-2.35 1.05-3.11z""/></svg>");

    public static MarkupString Microsoft => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M11.4 24H0V12.6h11.4V24zM24 24H12.6V12.6H24V24zM11.4 11.4H0V0h11.4v11.4zm12.6 0H12.6V0H24v11.4z""/></svg>");

    public static MarkupString Google => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z""/><path d=""M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z""/><path d=""M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z""/><path d=""M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z""/></svg>");

    public static MarkupString Amazon => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M.045 18.02c.072-.116.187-.124.348-.022 3.636 2.11 7.594 3.166 11.87 3.166 2.852 0 5.668-.533 8.447-1.595l.315-.14c.138-.06.234-.1.293-.13.226-.088.39-.046.502.123.086.131.094.27-.008.413-.182.256-.52.544-1.013.862-1.08.696-2.32 1.273-3.722 1.733-1.404.46-2.812.69-4.223.69-2.177 0-4.266-.388-6.263-1.163-1.998-.775-3.744-1.81-5.24-3.108a.405.405 0 0 1-.134-.309c0-.093.034-.18.1-.257l.728-.863zM6.093 14.12c0-.78.195-1.51.584-2.188a4.144 4.144 0 0 1 1.628-1.567c.62-.362 1.414-.652 2.384-.872.97-.22 2.127-.378 3.47-.473l.095-.006V8.4c0-.756-.09-1.283-.27-1.58-.18-.3-.538-.448-1.072-.448-.447 0-.8.112-1.06.336-.26.224-.44.61-.54 1.155-.075.406-.24.644-.496.713l-2.735-.338c-.272-.063-.408-.195-.408-.395 0-.063.01-.135.03-.217.176-.85.486-1.573.93-2.17.443-.596 1.03-1.054 1.757-1.372.728-.318 1.59-.476 2.59-.476 1.188 0 2.143.163 2.865.49.722.326 1.24.782 1.554 1.37.313.587.47 1.37.47 2.346v5.197c0 .37.045.667.133.895.088.228.196.387.326.48.13.09.34.167.633.228.178.036.267.145.267.327v2.12c0 .2-.09.32-.266.365l-.688.152c-.376.085-.703.127-.98.127-.71 0-1.28-.206-1.71-.62-.43-.412-.71-.984-.84-1.716-.83 1.527-2.154 2.29-3.97 2.29-.913 0-1.706-.27-2.38-.815-.674-.543-1.01-1.362-1.01-2.457zm5.14-.88c.31-.186.553-.426.73-.72.177-.294.266-.664.266-1.11v-.617l-1.17.086c-1.188.085-2.002.275-2.443.573-.44.297-.66.716-.66 1.256 0 .406.127.72.38.94.253.22.576.33.97.33.462 0 .882-.146 1.26-.44l.666-.298z""/><path d=""M22.438 19.77c.406-.31.763-.408 1.07-.3.306.11.46.37.46.78 0 .166-.023.34-.068.52-.286 1.14-.943 1.71-1.97 1.71-.406 0-.76-.113-1.06-.34-.3-.225-.55-.64-.75-1.24l-.23-.68c-.11-.34-.24-.55-.39-.64-.15-.09-.37-.08-.66.04l-.23.1c-.25.11-.41.13-.48.05-.07-.08-.05-.23.05-.45l.97-2.16c.1-.22.23-.34.38-.36.15-.02.37.04.65.18l.28.14c.31.16.53.2.66.12.13-.08.23-.27.3-.57l.03-.14c.04-.15.09-.25.15-.3.06-.05.15-.05.26 0l1.19.53c.21.09.31.23.31.41 0 .06-.01.13-.04.2l-.42 1.08c-.08.21-.05.37.08.49.13.12.34.18.63.18l.15-.01.15-.01c.19-.01.32.02.4.1.08.08.09.22.03.42l-.51 1.47z""/></svg>");

    public static MarkupString Tesla => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M12 5.362l2.475-3.026s4.245.09 8.471 2.054c-1.082 1.636-3.231 2.438-3.231 2.438-.146-1.439-1.154-1.79-4.354-1.79L12 24 8.619 5.034c-3.18 0-4.188.354-4.335 1.792 0 0-2.148-.795-3.229-2.43C5.28 2.431 9.525 2.34 9.525 2.34L12 5.362zm0-3.676c3.173 0 6.852.477 9.777 1.564.632-.37 1.096-.704 1.223-.856C18.981.274 13.676 0 12 0c-1.676 0-6.981.274-11 2.394.127.152.591.487 1.223.856 2.925-1.087 6.604-1.564 9.777-1.564z""/></svg>");

    public static MarkupString Meta => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M6.915 4.03c-1.968 0-3.683 1.28-4.871 3.113C.704 9.208 0 11.883 0 14.449c0 .706.07 1.369.21 1.973a6.624 6.624 0 0 0 .265.86c.196.478.463.9.795 1.25.332.351.733.628 1.2.807.467.178.99.264 1.55.264.551 0 1.06-.09 1.508-.262.448-.172.844-.422 1.186-.745.342-.324.635-.705.878-1.13.243-.425.446-.883.608-1.366.162-.482.292-.976.392-1.471.1-.496.178-.987.236-1.456.057-.468.1-.903.127-1.286.027-.383.041-.694.041-.902 0-.38-.022-.835-.066-1.345a18.322 18.322 0 0 0-.224-1.639 23.926 23.926 0 0 0-.41-1.866 18.6 18.6 0 0 0-.572-1.73c-.207-.51-.446-.993-.715-1.425a6.262 6.262 0 0 0-.876-1.076 3.725 3.725 0 0 0-.977-.702 2.451 2.451 0 0 0-1.04-.234zm10.17 0c-.365 0-.71.078-1.04.234a3.726 3.726 0 0 0-.977.702 6.261 6.261 0 0 0-.876 1.076c-.27.432-.508.914-.715 1.424-.207.51-.398 1.09-.571 1.73a23.924 23.924 0 0 0-.41 1.867 18.32 18.32 0 0 0-.225 1.639c-.044.51-.065.964-.065 1.345 0 .208.013.519.04.902.028.383.07.818.127 1.286.058.469.137.96.237 1.456.1.495.23.99.391 1.47.163.484.366.942.609 1.367.243.425.536.806.878 1.13.342.323.738.573 1.186.745.448.172.957.262 1.509.262.56 0 1.082-.086 1.549-.264.467-.179.868-.456 1.2-.807.332-.35.6-.772.795-1.25.096-.233.183-.49.265-.86.14-.603.21-1.267.21-1.973 0-2.566-.704-5.24-2.044-7.306-1.188-1.833-2.903-3.113-4.871-3.113zM12 10.148c-.432 0-.827.165-1.168.488-.34.322-.608.78-.804 1.33-.196.55-.294 1.194-.294 1.893 0 .7.098 1.343.294 1.893.196.55.464 1.008.804 1.33.341.323.736.488 1.168.488.432 0 .827-.165 1.167-.487.34-.323.609-.781.805-1.331.196-.55.293-1.193.293-1.893 0-.7-.097-1.343-.293-1.893-.196-.55-.464-1.008-.805-1.33-.34-.323-.735-.488-1.167-.488z""/></svg>");

    public static MarkupString Nvidia => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M8.948 8.798v-1.43a6.7 6.7 0 0 1 .424-.018c3.922-.124 6.493 3.374 6.493 3.374s-2.774 3.851-5.75 3.851c-.424 0-.814-.062-1.167-.171V9.395c1.244.132 1.488.64 2.227 1.694l1.663-1.4S10.99 8.675 8.948 8.798zm0-5.191v2.342l.424-.016c5.518-.174 9.156 4.678 9.156 4.678s-4.107 5.203-8.384 5.203a6.11 6.11 0 0 1-1.196-.12v1.382c.357.052.721.083 1.092.083 3.839 0 6.905-2.04 9.76-4.482.466.372 2.375 1.286 2.767 1.685-2.578 2.17-8.574 4.203-12.423 4.203-.4 0-.787-.027-1.196-.078V22.5H0V3.607h8.948zm0 9.368v1.415c-2.825-.453-3.61-3.218-3.61-3.218s1.376-1.483 3.61-1.64v1.53h-.004c-1.246-.132-2.224.96-2.224.96s.538 1.762 2.228.953zM2.864 9.433c0 .006 1.474-1.905 4.177-2.17V5.806c-3.168.327-5.353 2.6-5.353 2.6s1.263 5.823 5.353 6.521v-1.553c-2.77-.584-4.177-3.94-4.177-3.94z""/></svg>");

    public static MarkupString Netflix => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M5.398 0v.006c3.028 8.556 5.37 15.175 8.348 23.596 2.344.058 4.85.398 4.854.398-2.8-7.924-5.923-16.747-8.487-24zm8.489 0v9.63L18.6 22.951c-.043-7.86-.004-15.913.002-22.95zM5.398 1.05V24c1.873-.225 2.81-.312 4.715-.398v-9.22z""/></svg>");

    #endregion

    #region Metric Icons

    public static MarkupString Company => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M6 22V4a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v18Z""/><path d=""M6 12H4a2 2 0 0 0-2 2v6a2 2 0 0 0 2 2h2""/><path d=""M18 9h2a2 2 0 0 1 2 2v9a2 2 0 0 1-2 2h-2""/><path d=""M10 6h4""/><path d=""M10 10h4""/><path d=""M10 14h4""/><path d=""M10 18h4""/></svg>");

    public static MarkupString Revenue => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><line x1=""12"" y1=""1"" x2=""12"" y2=""23""/><path d=""M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6""/></svg>");

    public static MarkupString Users => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2""/><circle cx=""9"" cy=""7"" r=""4""/><path d=""M22 21v-2a4 4 0 0 0-3-3.87""/><path d=""M16 3.13a4 4 0 0 1 0 7.75""/></svg>");

    public static MarkupString Growth => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><polyline points=""23 6 13.5 15.5 8.5 10.5 1 18""/><polyline points=""17 6 23 6 23 12""/></svg>");

    public static MarkupString Rating => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""currentColor""><polygon points=""12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2""/></svg>");

    public static MarkupString MarketCap => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M3 3v18h18""/><path d=""m19 9-5 5-4-4-3 3""/></svg>");

    public static MarkupString Movies => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""2"" y=""2"" width=""20"" height=""20"" rx=""2.18"" ry=""2.18""/><line x1=""7"" y1=""2"" x2=""7"" y2=""22""/><line x1=""17"" y1=""2"" x2=""17"" y2=""22""/><line x1=""2"" y1=""12"" x2=""22"" y2=""12""/><line x1=""2"" y1=""7"" x2=""7"" y2=""7""/><line x1=""2"" y1=""17"" x2=""7"" y2=""17""/><line x1=""17"" y1=""17"" x2=""22"" y2=""17""/><line x1=""17"" y1=""7"" x2=""22"" y2=""7""/></svg>");

    public static MarkupString Themes => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""13.5"" cy=""6.5"" r=""0.5"" fill=""currentColor""/><circle cx=""17.5"" cy=""10.5"" r=""0.5"" fill=""currentColor""/><circle cx=""8.5"" cy=""7.5"" r=""0.5"" fill=""currentColor""/><circle cx=""6.5"" cy=""12.5"" r=""0.5"" fill=""currentColor""/><path d=""M12 2C6.5 2 2 6.5 2 12s4.5 10 10 10c.926 0 1.648-.746 1.648-1.688 0-.437-.18-.835-.437-1.125-.29-.289-.438-.652-.438-1.125a1.64 1.64 0 0 1 1.668-1.668h1.996c3.051 0 5.555-2.503 5.555-5.555C21.965 6.012 17.461 2 12 2z""/></svg>");

    public static MarkupString FPS => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><polygon points=""13 2 3 14 12 14 11 22 21 10 12 10 13 2""/></svg>");

    public static MarkupString Layout => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""3"" y=""3"" width=""18"" height=""18"" rx=""2"" ry=""2""/><line x1=""3"" y1=""9"" x2=""21"" y2=""9""/><line x1=""9"" y1=""21"" x2=""9"" y2=""9""/></svg>");

    public static MarkupString Uptime => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><polyline points=""20 6 9 17 4 12""/></svg>");

    public static MarkupString Support => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z""/></svg>");

    public static MarkupString Components => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""3"" y=""3"" width=""7"" height=""7""/><rect x=""14"" y=""3"" width=""7"" height=""7""/><rect x=""14"" y=""14"" width=""7"" height=""7""/><rect x=""3"" y=""14"" width=""7"" height=""7""/></svg>");

    public static MarkupString Dependencies => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z""/><polyline points=""3.27 6.96 12 12.01 20.73 6.96""/><line x1=""12"" y1=""22.08"" x2=""12"" y2=""12""/></svg>");

    #endregion

    #region UI Icons

    public static MarkupString Lightning => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><polygon points=""13 2 3 14 12 14 11 22 21 10 12 10 13 2""></polygon></svg>");

    public static MarkupString GitHub => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""currentColor""><path d=""M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z""/></svg>");

    public static MarkupString Unknown => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2""><circle cx=""12"" cy=""12"" r=""10""/><path d=""M12 16v-4""/><path d=""M12 8h.01""/></svg>");

    #endregion

    #region Navigation Icons

    public static MarkupString Package => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""m7.5 4.27 9 5.15""/><path d=""M21 8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16Z""/><path d=""m3.3 7 8.7 5 8.7-5""/><path d=""M12 22V12""/></svg>");

    public static MarkupString Rocket => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M4.5 16.5c-1.5 1.26-2 5-2 5s3.74-.5 5-2c.71-.84.7-2.13-.09-2.91a2.18 2.18 0 0 0-2.91-.09z""/><path d=""m12 15-3-3a22 22 0 0 1 2-3.95A12.88 12.88 0 0 1 22 2c0 2.72-.78 7.5-6 11a22.35 22.35 0 0 1-4 2z""/><path d=""M9 12H4s.55-3.03 2-4c1.62-1.08 5 0 5 0""/><path d=""M12 15v5s3.03-.55 4-2c1.08-1.62 0-5 0-5""/></svg>");

    public static MarkupString Carousel => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect width=""12"" height=""14"" x=""6"" y=""5"" rx=""2""/><path d=""M2 7v10""/><path d=""M22 7v10""/></svg>");

    public static MarkupString BentoGrid => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect width=""7"" height=""7"" x=""3"" y=""3"" rx=""1""/><rect width=""7"" height=""7"" x=""14"" y=""3"" rx=""1""/><rect width=""7"" height=""7"" x=""14"" y=""14"" rx=""1""/><rect width=""7"" height=""7"" x=""3"" y=""14"" rx=""1""/></svg>");

    public static MarkupString Book => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1 0-5H20""/></svg>");

    public static MarkupString Palette => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""13.5"" cy=""6.5"" r="".5"" fill=""currentColor""/><circle cx=""17.5"" cy=""10.5"" r="".5"" fill=""currentColor""/><circle cx=""8.5"" cy=""7.5"" r="".5"" fill=""currentColor""/><circle cx=""6.5"" cy=""12.5"" r="".5"" fill=""currentColor""/><path d=""M12 2C6.5 2 2 6.5 2 12s4.5 10 10 10c.926 0 1.648-.746 1.648-1.688 0-.437-.18-.835-.437-1.125-.29-.289-.438-.652-.438-1.125a1.64 1.64 0 0 1 1.668-1.668h1.996c3.051 0 5.555-2.503 5.555-5.555C21.965 6.012 17.461 2 12 2z""/></svg>");

    public static MarkupString Settings => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M12.22 2h-.44a2 2 0 0 0-2 2v.18a2 2 0 0 1-1 1.73l-.43.25a2 2 0 0 1-2 0l-.15-.08a2 2 0 0 0-2.73.73l-.22.38a2 2 0 0 0 .73 2.73l.15.1a2 2 0 0 1 1 1.72v.51a2 2 0 0 1-1 1.74l-.15.09a2 2 0 0 0-.73 2.73l.22.38a2 2 0 0 0 2.73.73l.15-.08a2 2 0 0 1 2 0l.43.25a2 2 0 0 1 1 1.73V20a2 2 0 0 0 2 2h.44a2 2 0 0 0 2-2v-.18a2 2 0 0 1 1-1.73l.43-.25a2 2 0 0 1 2 0l.15.08a2 2 0 0 0 2.73-.73l.22-.39a2 2 0 0 0-.73-2.73l-.15-.08a2 2 0 0 1-1-1.74v-.5a2 2 0 0 1 1-1.74l.15-.09a2 2 0 0 0 .73-2.73l-.22-.38a2 2 0 0 0-2.73-.73l-.15.08a2 2 0 0 1-2 0l-.43-.25a2 2 0 0 1-1-1.73V4a2 2 0 0 0-2-2z""/><circle cx=""12"" cy=""12"" r=""3""/></svg>");

    public static MarkupString Code => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><polyline points=""16 18 22 12 16 6""/><polyline points=""8 6 2 12 8 18""/></svg>");

    public static MarkupString Lightbulb => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M15 14c.2-1 .7-1.7 1.5-2.5 1-.9 1.5-2.2 1.5-3.5A6 6 0 0 0 6 8c0 1 .2 2.2 1.5 3.5.7.7 1.3 1.5 1.5 2.5""/><path d=""M9 18h6""/><path d=""M10 22h4""/></svg>");

    #endregion

    #region Theme Icons

    public static MarkupString Sun => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""4""/><path d=""M12 2v2""/><path d=""M12 20v2""/><path d=""m4.93 4.93 1.41 1.41""/><path d=""m17.66 17.66 1.41 1.41""/><path d=""M2 12h2""/><path d=""M20 12h2""/><path d=""m6.34 17.66-1.41 1.41""/><path d=""m19.07 4.93-1.41 1.41""/></svg>");

    public static MarkupString Moon => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9Z""/></svg>");

    public static MarkupString Crystal => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M12 2 2 7l10 5 10-5-10-5Z""/><path d=""m2 17 10 5 10-5""/><path d=""m2 12 10 5 10-5""/></svg>");

    public static MarkupString Minus => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M5 12h14""/></svg>");

    #endregion

    #region Doc Section Icons

    public static MarkupString Sparkles => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""m12 3-1.912 5.813a2 2 0 0 1-1.275 1.275L3 12l5.813 1.912a2 2 0 0 1 1.275 1.275L12 21l1.912-5.813a2 2 0 0 1 1.275-1.275L21 12l-5.813-1.912a2 2 0 0 1-1.275-1.275L12 3Z""/><path d=""M5 3v4""/><path d=""M19 17v4""/><path d=""M3 5h4""/><path d=""M17 19h4""/></svg>");

    public static MarkupString Target => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""10""/><circle cx=""12"" cy=""12"" r=""6""/><circle cx=""12"" cy=""12"" r=""2""/></svg>");

    public static MarkupString Film => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect width=""18"" height=""18"" x=""3"" y=""3"" rx=""2""/><path d=""M7 3v18""/><path d=""M3 7.5h4""/><path d=""M3 12h18""/><path d=""M3 16.5h4""/><path d=""M17 3v18""/><path d=""M17 7.5h4""/><path d=""M17 16.5h4""/></svg>");

    public static MarkupString ShoppingBag => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M6 2 3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4Z""/><path d=""M3 6h18""/><path d=""M16 10a4 4 0 0 1-8 0""/></svg>");

    public static MarkupString Eye => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M2 12s3-7 10-7 10 7 10 7-3 7-10 7-10-7-10-7Z""/><circle cx=""12"" cy=""12"" r=""3""/></svg>");

    public static MarkupString Masks => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M2 12a5 5 0 0 0 5 5 8 8 0 0 1 5 2 8 8 0 0 1 5-2 5 5 0 0 0 5-5V7h-5a8 8 0 0 0-5 2 8 8 0 0 0-5-2H2Z""/><path d=""M6 11c1.5 0 3 .5 3 2-2 0-3 0-3-2Z""/><path d=""M18 11c-1.5 0-3 .5-3 2 2 0 3 0 3-2Z""/></svg>");

    public static MarkupString Hourglass => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M5 22h14""/><path d=""M5 2h14""/><path d=""M17 22v-4.172a2 2 0 0 0-.586-1.414L12 12l-4.414 4.414A2 2 0 0 0 7 17.828V22""/><path d=""M7 2v4.172a2 2 0 0 0 .586 1.414L12 12l4.414-4.414A2 2 0 0 0 17 6.172V2""/></svg>");

    public static MarkupString MailboxEmpty => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M22 17a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V9.5C2 7 4 5 6.5 5H18c2.2 0 4 1.8 4 4v8Z""/><polyline points=""15,9 18,9 18,11""/><path d=""M6.5 5C9 5 11 7 11 9.5V17a2 2 0 0 1-2 2""/><line x1=""6"" x2=""7"" y1=""10"" y2=""10""/></svg>");

    public static MarkupString Smartphone => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect width=""14"" height=""20"" x=""5"" y=""2"" rx=""2"" ry=""2""/><path d=""M12 18h.01""/></svg>");

    public static MarkupString UsersGroup => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2""/><circle cx=""9"" cy=""7"" r=""4""/><path d=""M22 21v-2a4 4 0 0 0-3-3.87""/><path d=""M16 3.13a4 4 0 0 1 0 7.75""/></svg>");

    public static MarkupString CircleCheck => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""10""/><path d=""m9 12 2 2 4-4""/></svg>");

    public static MarkupString Download => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4""/><polyline points=""7 10 12 15 17 10""/><line x1=""12"" x2=""12"" y1=""15"" y2=""3""/></svg>");

    public static MarkupString Tag => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M12.586 2.586A2 2 0 0 0 11.172 2H4a2 2 0 0 0-2 2v7.172a2 2 0 0 0 .586 1.414l8.704 8.704a2.426 2.426 0 0 0 3.42 0l6.58-6.58a2.426 2.426 0 0 0 0-3.42z""/><circle cx=""7.5"" cy=""7.5"" r="".5"" fill=""currentColor""/></svg>");

    public static MarkupString Image => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect width=""18"" height=""18"" x=""3"" y=""3"" rx=""2"" ry=""2""/><circle cx=""9"" cy=""9"" r=""2""/><path d=""m21 15-3.086-3.086a2 2 0 0 0-2.828 0L6 21""/></svg>");

    public static MarkupString Pen => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M17 3a2.85 2.83 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z""/></svg>");

    public static MarkupString FileText => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M15 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V7Z""/><path d=""M14 2v4a2 2 0 0 0 2 2h4""/><path d=""M10 9H8""/><path d=""M16 13H8""/><path d=""M16 17H8""/></svg>");

    public static MarkupString Hand => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M18 11V6a2 2 0 0 0-2-2 2 2 0 0 0-2 2""/><path d=""M14 10V4a2 2 0 0 0-2-2 2 2 0 0 0-2 2v2""/><path d=""M10 10.5V6a2 2 0 0 0-2-2 2 2 0 0 0-2 2v8""/><path d=""M18 8a2 2 0 1 1 4 0v6a8 8 0 0 1-8 8h-2c-2.8 0-4.5-.86-5.99-2.34l-3.6-3.6a2 2 0 0 1 2.83-2.82L7 15""/></svg>");

    public static MarkupString AlertTriangle => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z""/><path d=""M12 9v4""/><path d=""M12 17h.01""/></svg>");

    public static MarkupString Ruler => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M21.3 15.3a2.4 2.4 0 0 1 0 3.4l-2.6 2.6a2.4 2.4 0 0 1-3.4 0L2.7 8.7a2.41 2.41 0 0 1 0-3.4l2.6-2.6a2.41 2.41 0 0 1 3.4 0Z""/><path d=""m14.5 12.5 2-2""/><path d=""m11.5 9.5 2-2""/><path d=""m8.5 6.5 2-2""/><path d=""m17.5 15.5 2-2""/></svg>");

    public static MarkupString BookOpen => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z""/><path d=""M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z""/></svg>");

    public static MarkupString Library => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""m16 6 4 14""/><path d=""M12 6v14""/><path d=""M8 8v12""/><path d=""M4 4v16""/></svg>");

    public static MarkupString BarChart => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><line x1=""12"" x2=""12"" y1=""20"" y2=""10""/><line x1=""18"" x2=""18"" y1=""20"" y2=""4""/><line x1=""6"" x2=""6"" y1=""20"" y2=""16""/></svg>");

    public static MarkupString Wrench => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z""/></svg>");

    public static MarkupString CircleOne => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""10""/><path d=""M12 8v8""/><path d=""M10 10h2""/></svg>");

    public static MarkupString CircleTwo => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""10""/><path d=""M10 8h3a1 1 0 0 1 1 1v1a1 1 0 0 1-1 1h-2a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1h3""/></svg>");

    public static MarkupString CircleThree => new(@"<svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""10""/><path d=""M10 8h3a1 1 0 0 1 1 1v1a1 1 0 0 1-1 1h-2""/><path d=""M11 12h2a1 1 0 0 1 1 1v1a1 1 0 0 1-1 1h-3""/></svg>");

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets a company logo by name. Returns Unknown icon if not found.
    /// </summary>
    public static MarkupString GetCompanyLogo(string? companyName)
    {
        if (string.IsNullOrEmpty(companyName))
            return Unknown;

        return companyName switch
        {
            "Apple" => Apple,
            "Microsoft" => Microsoft,
            "Google" => Google,
            "Amazon" => Amazon,
            "Tesla" => Tesla,
            "Meta" => Meta,
            "Nvidia" => Nvidia,
            "Netflix" => Netflix,
            _ => Unknown
        };
    }

    /// <summary>
    /// Gets a metric icon by name. Returns Unknown icon if not found.
    /// </summary>
    public static MarkupString GetMetricIcon(string? metricName)
    {
        if (string.IsNullOrEmpty(metricName))
            return Unknown;

        return metricName switch
        {
            "Company" => Company,
            "Revenue" => Revenue,
            "Users" => Users,
            "Growth" => Growth,
            "Rating" => Rating,
            "MarketCap" => MarketCap,
            "Movies" => Movies,
            "Themes" => Themes,
            "FPS" => FPS,
            "Layout" => Layout,
            "Uptime" => Uptime,
            "Support" => Support,
            "Components" => Components,
            "Dependencies" => Dependencies,
            _ => Unknown
        };
    }

    #endregion
}
