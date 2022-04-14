using AuraServiceLib;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualGrid.Asus.Enums;
using VirtualGrid.Interfaces;

namespace VirtualGrid.Asus
{
    /// <summary>
    /// An adapter to convert <see cref="IVirtualLedGrid"/> into Asus Rog Strix G15 (2021) compatible effect.
    /// </summary>
    public class AsusRogStrix_G15_2021_Adapter : IPhysicalDeviceAdapter
    {
        private readonly IAuraSdk2 _sdk;
        private readonly IAuraSyncDevice _notebookKeyboard;
        private readonly RogStrixKeyboardKey[][] _representor = new RogStrixKeyboardKey[9][]
        {
            new [] {(RogStrixKeyboardKey)0,(RogStrixKeyboardKey)1,(RogStrixKeyboardKey)2,(RogStrixKeyboardKey)3,(RogStrixKeyboardKey)4,(RogStrixKeyboardKey)5,(RogStrixKeyboardKey)6,(RogStrixKeyboardKey)7,(RogStrixKeyboardKey)8,(RogStrixKeyboardKey)9,(RogStrixKeyboardKey)10,(RogStrixKeyboardKey)11,(RogStrixKeyboardKey)12,(RogStrixKeyboardKey)13,(RogStrixKeyboardKey)14,(RogStrixKeyboardKey)15,(RogStrixKeyboardKey)16,(RogStrixKeyboardKey)17,(RogStrixKeyboardKey)18,(RogStrixKeyboardKey)19,(RogStrixKeyboardKey)20,},
            new [] {(RogStrixKeyboardKey)21,(RogStrixKeyboardKey)22,(RogStrixKeyboardKey)23,(RogStrixKeyboardKey)24,(RogStrixKeyboardKey)25,(RogStrixKeyboardKey)26,(RogStrixKeyboardKey)27,(RogStrixKeyboardKey)28,(RogStrixKeyboardKey)29,(RogStrixKeyboardKey)30,(RogStrixKeyboardKey)31,(RogStrixKeyboardKey)32,(RogStrixKeyboardKey)33,(RogStrixKeyboardKey)34,(RogStrixKeyboardKey)35,(RogStrixKeyboardKey)36,(RogStrixKeyboardKey)37,(RogStrixKeyboardKey)38,(RogStrixKeyboardKey)39,(RogStrixKeyboardKey)40,(RogStrixKeyboardKey)41,},
            new [] {(RogStrixKeyboardKey)42,(RogStrixKeyboardKey)43,(RogStrixKeyboardKey)44,(RogStrixKeyboardKey)45,(RogStrixKeyboardKey)46,(RogStrixKeyboardKey)47,(RogStrixKeyboardKey)48,(RogStrixKeyboardKey)49,(RogStrixKeyboardKey)50,(RogStrixKeyboardKey)51,(RogStrixKeyboardKey)52,(RogStrixKeyboardKey)53,(RogStrixKeyboardKey)54,(RogStrixKeyboardKey)55,(RogStrixKeyboardKey)56,(RogStrixKeyboardKey)57,(RogStrixKeyboardKey)58,(RogStrixKeyboardKey)59,(RogStrixKeyboardKey)60,(RogStrixKeyboardKey)61,(RogStrixKeyboardKey)62,},
            new [] {(RogStrixKeyboardKey)63,(RogStrixKeyboardKey)64,(RogStrixKeyboardKey)65,(RogStrixKeyboardKey)66,(RogStrixKeyboardKey)67,(RogStrixKeyboardKey)68,(RogStrixKeyboardKey)69,(RogStrixKeyboardKey)70,(RogStrixKeyboardKey)71,(RogStrixKeyboardKey)72,(RogStrixKeyboardKey)73,(RogStrixKeyboardKey)74,(RogStrixKeyboardKey)75,(RogStrixKeyboardKey)76,(RogStrixKeyboardKey)77,(RogStrixKeyboardKey)78,(RogStrixKeyboardKey)79,(RogStrixKeyboardKey)80,(RogStrixKeyboardKey)81,(RogStrixKeyboardKey)82,(RogStrixKeyboardKey)83,},
            new [] {(RogStrixKeyboardKey)84,(RogStrixKeyboardKey)85,(RogStrixKeyboardKey)86,(RogStrixKeyboardKey)87,(RogStrixKeyboardKey)88,(RogStrixKeyboardKey)89,(RogStrixKeyboardKey)90,(RogStrixKeyboardKey)91,(RogStrixKeyboardKey)92,(RogStrixKeyboardKey)93,(RogStrixKeyboardKey)94,(RogStrixKeyboardKey)95,(RogStrixKeyboardKey)96,(RogStrixKeyboardKey)97,(RogStrixKeyboardKey)98,(RogStrixKeyboardKey)99,(RogStrixKeyboardKey)100,(RogStrixKeyboardKey)101,(RogStrixKeyboardKey)102,(RogStrixKeyboardKey)103,(RogStrixKeyboardKey)104,},
            new [] {(RogStrixKeyboardKey)105,(RogStrixKeyboardKey)106,(RogStrixKeyboardKey)107,(RogStrixKeyboardKey)108,(RogStrixKeyboardKey)109,(RogStrixKeyboardKey)110,(RogStrixKeyboardKey)111,(RogStrixKeyboardKey)112,(RogStrixKeyboardKey)113,(RogStrixKeyboardKey)114,(RogStrixKeyboardKey)115,(RogStrixKeyboardKey)116,(RogStrixKeyboardKey)117,(RogStrixKeyboardKey)118,(RogStrixKeyboardKey)119,(RogStrixKeyboardKey)120,(RogStrixKeyboardKey)121,(RogStrixKeyboardKey)122,(RogStrixKeyboardKey)123,(RogStrixKeyboardKey)124,(RogStrixKeyboardKey)125,},
            new [] {(RogStrixKeyboardKey)126,(RogStrixKeyboardKey)127,(RogStrixKeyboardKey)128,(RogStrixKeyboardKey)129,(RogStrixKeyboardKey)130,(RogStrixKeyboardKey)131,(RogStrixKeyboardKey)132,(RogStrixKeyboardKey)133,(RogStrixKeyboardKey)134,(RogStrixKeyboardKey)135,(RogStrixKeyboardKey)136,(RogStrixKeyboardKey)137,(RogStrixKeyboardKey)138,(RogStrixKeyboardKey)139,(RogStrixKeyboardKey)140,(RogStrixKeyboardKey)141,(RogStrixKeyboardKey)142,(RogStrixKeyboardKey)143,(RogStrixKeyboardKey)144,(RogStrixKeyboardKey)145,(RogStrixKeyboardKey)146,},
            new [] {(RogStrixKeyboardKey)147,(RogStrixKeyboardKey)148,(RogStrixKeyboardKey)149,(RogStrixKeyboardKey)150,(RogStrixKeyboardKey)151,(RogStrixKeyboardKey)152,(RogStrixKeyboardKey)153,(RogStrixKeyboardKey)154,(RogStrixKeyboardKey)155,(RogStrixKeyboardKey)156,(RogStrixKeyboardKey)157,(RogStrixKeyboardKey)158,(RogStrixKeyboardKey)159,(RogStrixKeyboardKey)160,(RogStrixKeyboardKey)161,(RogStrixKeyboardKey)162,(RogStrixKeyboardKey)163,(RogStrixKeyboardKey)164,(RogStrixKeyboardKey)165,(RogStrixKeyboardKey)166,(RogStrixKeyboardKey)167,},
            new [] {(RogStrixKeyboardKey)168,(RogStrixKeyboardKey)169,(RogStrixKeyboardKey)170,(RogStrixKeyboardKey)171,(RogStrixKeyboardKey)172,(RogStrixKeyboardKey)173,(RogStrixKeyboardKey)174,(RogStrixKeyboardKey)175,(RogStrixKeyboardKey)176,(RogStrixKeyboardKey)177,(RogStrixKeyboardKey)178,(RogStrixKeyboardKey)179,(RogStrixKeyboardKey)180,(RogStrixKeyboardKey)181,(RogStrixKeyboardKey)182,(RogStrixKeyboardKey)183,(RogStrixKeyboardKey)184,(RogStrixKeyboardKey)185,(RogStrixKeyboardKey)186,(RogStrixKeyboardKey)(-1),(RogStrixKeyboardKey)(-1),},
        };

        public string Name => "Asus Rog Strix Laptop";

        public bool Initialized { get; }

        public int RowCount => 9;

        public int ColumnCount => 21;

        /// <summary>
        /// Constructor for ASUS Rog Strix G15 (2021) laptop adapter.
        /// </summary>
        public AsusRogStrix_G15_2021_Adapter()
        {
            try
            {
                var sdk = (IAuraSdk2)new AuraSdk();
                sdk.SwitchMode();
                var devices = sdk.Enumerate(528384);
                this._sdk = sdk;
                this._notebookKeyboard = devices.Count > 0 ? devices[0] : throw new Exception("No Asus device attached.");
                this.Initialized = true;
            }
            catch (Exception)
            {
                this.Initialized = false;
            }
        }

        /// <inheritdoc/>
        public Task ApplyAsync(IVirtualLedGrid virtualGrid, CancellationToken cancellationToken = default)
        {
            if (!this.Initialized)
            {
                return Task.CompletedTask;
            }

            for (var row = 0; row < virtualGrid.RowCount; row++)
            {
                var physicalRow = _representor[row];

                for (var col = 0; col < virtualGrid.ColumnCount; col++)
                {
                    var cellColor = virtualGrid[col, row];

                    if (cellColor == null)
                    {
                        continue;
                    }

                    var key = physicalRow[col];

                    if (IsInvalidKey(key))
                    {
                        continue;
                    }

                    var color = ToUint(cellColor.Value);

                    _notebookKeyboard.Lights[(int)key].Color = color;
                }
            }

            _notebookKeyboard.Apply();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this._sdk?.ReleaseControl(0);
        }

        private static uint ToUint(Color color) => (uint)(color.B << 16 | color.G << 8 | color.R);

        private static bool IsInvalidKey(RogStrixKeyboardKey key)
        {
            switch (key)
            {
                case RogStrixKeyboardKey.GhostKey:
                case RogStrixKeyboardKey.Invalid0_1:
                case RogStrixKeyboardKey.Invalid0_2:
                case RogStrixKeyboardKey.Invalid0_3:
                case RogStrixKeyboardKey.Invalid0_4:
                case RogStrixKeyboardKey.Invalid0_5:
                case RogStrixKeyboardKey.Invalid0_6:
                case RogStrixKeyboardKey.Invalid0_7:
                case RogStrixKeyboardKey.Invalid0_8:
                case RogStrixKeyboardKey.Invalid0_9:
                case RogStrixKeyboardKey.Invalid0_10:
                case RogStrixKeyboardKey.Invalid0_11:
                case RogStrixKeyboardKey.Invalid0_12:
                case RogStrixKeyboardKey.Invalid0_13:
                case RogStrixKeyboardKey.Invalid0_14:
                case RogStrixKeyboardKey.Invalid0_15:
                case RogStrixKeyboardKey.Invalid0_16:
                case RogStrixKeyboardKey.Invalid1_1:
                case RogStrixKeyboardKey.Invalid1_2:
                case RogStrixKeyboardKey.Invalid1_3:
                case RogStrixKeyboardKey.Invalid1_4:
                case RogStrixKeyboardKey.Invalid1_5:
                case RogStrixKeyboardKey.Invalid1_6:
                case RogStrixKeyboardKey.Invalid1_7:
                case RogStrixKeyboardKey.Invalid2_1:
                case RogStrixKeyboardKey.Invalid2_2:
                case RogStrixKeyboardKey.Invalid2_3:
                case RogStrixKeyboardKey.Invalid2_4:
                case RogStrixKeyboardKey.Invalid3_1:
                case RogStrixKeyboardKey.Invalid3_2:
                case RogStrixKeyboardKey.Invalid3_3:
                case RogStrixKeyboardKey.Invalid3_4:
                case RogStrixKeyboardKey.Invalid3_5:
                case RogStrixKeyboardKey.Invalid3_6:
                case RogStrixKeyboardKey.Invalid4_1:
                case RogStrixKeyboardKey.Invalid4_2:
                case RogStrixKeyboardKey.Invalid4_3:
                case RogStrixKeyboardKey.Invalid4_4:
                case RogStrixKeyboardKey.Invalid4_5:
                case RogStrixKeyboardKey.Invalid5_1:
                case RogStrixKeyboardKey.Invalid5_2:
                case RogStrixKeyboardKey.Invalid5_3:
                case RogStrixKeyboardKey.Invalid5_4:
                case RogStrixKeyboardKey.Invalid5_5:
                case RogStrixKeyboardKey.Invalid5_6:
                case RogStrixKeyboardKey.Invalid6_1:
                case RogStrixKeyboardKey.Invalid6_2:
                case RogStrixKeyboardKey.Invalid6_3:
                case RogStrixKeyboardKey.Invalid6_4:
                case RogStrixKeyboardKey.Invalid6_5:
                case RogStrixKeyboardKey.Invalid6_6:
                case RogStrixKeyboardKey.Invalid6_7:
                case RogStrixKeyboardKey.Invalid6_8:
                case RogStrixKeyboardKey.Invalid6_9:
                case RogStrixKeyboardKey.Invalid7_1:
                case RogStrixKeyboardKey.Invalid7_2:
                case RogStrixKeyboardKey.Invalid7_3:
                case RogStrixKeyboardKey.Invalid7_4:
                case RogStrixKeyboardKey.Invalid7_5:
                case RogStrixKeyboardKey.Invalid7_6:
                case RogStrixKeyboardKey.Invalid7_7:
                case RogStrixKeyboardKey.Invalid7_8:
                case RogStrixKeyboardKey.Invalid7_9:
                case RogStrixKeyboardKey.Invalid7_10:
                case RogStrixKeyboardKey.Invalid7_11:
                case RogStrixKeyboardKey.Invalid7_12:
                case RogStrixKeyboardKey.Invalid7_13:
                case RogStrixKeyboardKey.Invalid7_14:
                case RogStrixKeyboardKey.Invalid7_15:
                case RogStrixKeyboardKey.Invalid7_16:
                case RogStrixKeyboardKey.Invalid7_17:
                case RogStrixKeyboardKey.Invalid7_18:
                    return true;
                default:
                    return false;
            }
        }
    }
}