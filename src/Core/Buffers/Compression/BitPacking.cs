// -----------------------------------------------------------------------
// <copyright file="BitPacking.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The bit packing.
/// </summary>
internal static class BitPacking
{
    private const int Mask1 = 0x01;
    private const int Mask2 = 0x03;
    private const int Mask3 = 0x07;
    private const int Mask4 = 0x0F;
    private const int Mask5 = 0x1F;
    private const int Mask6 = 0x3F;
    private const int Mask7 = 0x7F;
    private const int Mask8 = 0xFF;
    private const int Mask9 = 0x01FF;
    private const int Mask10 = 0x03FF;
    private const int Mask11 = 0x07FF;
    private const int Mask12 = 0x0FFF;
    private const int Mask13 = 0x1FFF;
    private const int Mask14 = 0x3FFF;
    private const int Mask15 = 0x7FFF;
    private const int Mask16 = 0xFFFF;
    private const int Mask17 = 0x01FFFF;
    private const int Mask18 = 0x03FFFF;
    private const int Mask19 = 0x07FFFF;
    private const int Mask20 = 0x0FFFFF;
    private const int Mask21 = 0x1FFFFF;
    private const int Mask22 = 0x3FFFFF;
    private const int Mask23 = 0x7FFFFF;
    private const int Mask24 = 0xFFFFFF;
    private const int Mask25 = 0x01FFFFFF;
    private const int Mask26 = 0x03FFFFFF;
    private const int Mask27 = 0x07FFFFFF;
    private const int Mask28 = 0x0FFFFFFF;
    private const int Mask29 = 0x1FFFFFFF;
    private const int Mask30 = 0x3FFFFFFF;
    private const int Mask31 = 0x7FFFFFFF;

    /// <summary>
    /// Pack 32 integers.
    /// </summary>
    /// <param name="source">source array.</param>
    /// <param name="destination">output array.</param>
    /// <param name="bit">number of bits to use per integer.</param>
    public static void Pack(ReadOnlySpan<int> source, Span<int> destination, int bit)
    {
        switch (bit)
        {
            case 0:
                break;
            case 1:
                Pack1(source, destination);
                break;
            case 2:
                Pack2(source, destination);
                break;
            case 3:
                Pack3(source, destination);
                break;
            case 4:
                Pack4(source, destination);
                break;
            case 5:
                Pack5(source, destination);
                break;
            case 6:
                Pack6(source, destination);
                break;
            case 7:
                Pack7(source, destination);
                break;
            case 8:
                Pack8(source, destination);
                break;
            case 9:
                Pack9(source, destination);
                break;
            case 10:
                Pack10(source, destination);
                break;
            case 11:
                Pack11(source, destination);
                break;
            case 12:
                Pack12(source, destination);
                break;
            case 13:
                Pack13(source, destination);
                break;
            case 14:
                Pack14(source, destination);
                break;
            case 15:
                Pack15(source, destination);
                break;
            case 16:
                Pack16(source, destination);
                break;
            case 17:
                Pack17(source, destination);
                break;
            case 18:
                Pack18(source, destination);
                break;
            case 19:
                Pack19(source, destination);
                break;
            case 20:
                Pack20(source, destination);
                break;
            case 21:
                Pack21(source, destination);
                break;
            case 22:
                Pack22(source, destination);
                break;
            case 23:
                Pack23(source, destination);
                break;
            case 24:
                Pack24(source, destination);
                break;
            case 25:
                Pack25(source, destination);
                break;
            case 26:
                Pack26(source, destination);
                break;
            case 27:
                Pack27(source, destination);
                break;
            case 28:
                Pack28(source, destination);
                break;
            case 29:
                Pack29(source, destination);
                break;
            case 30:
                Pack30(source, destination);
                break;
            case 31:
                Pack31(source, destination);
                break;
            case 32:
                Pack32(source, destination);
                break;
            default:
                throw new ArgumentException("Unsupported bit width.", paramName: nameof(bit));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack1(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask1)
                | ((source[1] & Mask1) << 1)
                | ((source[2] & Mask1) << 2)
                | ((source[3] & Mask1) << 3)
                | ((source[4] & Mask1) << 4)
                | ((source[5] & Mask1) << 5)
                | ((source[6] & Mask1) << 6)
                | ((source[7] & Mask1) << 7)
                | ((source[8] & Mask1) << 8)
                | ((source[9] & Mask1) << 9)
                | ((source[10] & Mask1) << 10)
                | ((source[11] & Mask1) << 11)
                | ((source[12] & Mask1) << 12)
                | ((source[13] & Mask1) << 13)
                | ((source[14] & Mask1) << 14)
                | ((source[15] & Mask1) << 15)
                | ((source[16] & Mask1) << 16)
                | ((source[17] & Mask1) << 17)
                | ((source[18] & Mask1) << 18)
                | ((source[19] & Mask1) << 19)
                | ((source[20] & Mask1) << 20)
                | ((source[21] & Mask1) << 21)
                | ((source[22] & Mask1) << 22)
                | ((source[23] & Mask1) << 23)
                | ((source[24] & Mask1) << 24)
                | ((source[25] & Mask1) << 25)
                | ((source[26] & Mask1) << 26)
                | ((source[27] & Mask1) << 27)
                | ((source[28] & Mask1) << 28)
                | ((source[29] & Mask1) << 29)
                | ((source[30] & Mask1) << 30)
                | (source[31] << 31);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack2(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask2)
                | ((source[1] & Mask2) << 2)
                | ((source[2] & Mask2) << 4)
                | ((source[3] & Mask2) << 6)
                | ((source[4] & Mask2) << 8)
                | ((source[5] & Mask2) << 10)
                | ((source[6] & Mask2) << 12)
                | ((source[7] & Mask2) << 14)
                | ((source[8] & Mask2) << 16)
                | ((source[9] & Mask2) << 18)
                | ((source[10] & Mask2) << 20)
                | ((source[11] & Mask2) << 22)
                | ((source[12] & Mask2) << 24)
                | ((source[13] & Mask2) << 26)
                | ((source[14] & Mask2) << 28)
                | (source[15] << 30);
            destination[1] = (source[16] & Mask2)
                | ((source[17] & Mask2) << 2)
                | ((source[18] & Mask2) << 4)
                | ((source[19] & Mask2) << 6)
                | ((source[20] & Mask2) << 8)
                | ((source[21] & Mask2) << 10)
                | ((source[22] & Mask2) << 12)
                | ((source[23] & Mask2) << 14)
                | ((source[24] & Mask2) << 16)
                | ((source[25] & Mask2) << 18)
                | ((source[26] & Mask2) << 20)
                | ((source[27] & Mask2) << 22)
                | ((source[28] & Mask2) << 24)
                | ((source[29] & Mask2) << 26)
                | ((source[30] & Mask2) << 28)
                | (source[31] << 30);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack3(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask3)
                | ((source[1] & Mask3) << 3)
                | ((source[2] & Mask3) << 6)
                | ((source[3] & Mask3) << 9)
                | ((source[4] & Mask3) << 12)
                | ((source[5] & Mask3) << 15)
                | ((source[6] & Mask3) << 18)
                | ((source[7] & Mask3) << 21)
                | ((source[8] & Mask3) << 24)
                | ((source[9] & Mask3) << 27)
                | (source[10] << 30);
            destination[1] = (int)(((uint)source[10] & Mask3) >> (3 - 1))
                | ((source[11] & Mask3) << 1)
                | ((source[12] & Mask3) << 4)
                | ((source[13] & Mask3) << 7)
                | ((source[14] & Mask3) << 10)
                | ((source[15] & Mask3) << 13)
                | ((source[16] & Mask3) << 16)
                | ((source[17] & Mask3) << 19)
                | ((source[18] & Mask3) << 22)
                | ((source[19] & Mask3) << 25)
                | ((source[20] & Mask3) << 28)
                | (source[21] << 31);
            destination[2] = (int)(((uint)source[21] & Mask3) >> (3 - 2))
                | ((source[22] & Mask3) << 2)
                | ((source[23] & Mask3) << 5)
                | ((source[24] & Mask3) << 8)
                | ((source[25] & Mask3) << 11)
                | ((source[26] & Mask3) << 14)
                | ((source[27] & Mask3) << 17)
                | ((source[28] & Mask3) << 20)
                | ((source[29] & Mask3) << 23)
                | ((source[30] & Mask3) << 26)
                | (source[31] << 29);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack4(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask4)
                | ((source[1] & Mask4) << 4)
                | ((source[2] & Mask4) << 8)
                | ((source[3] & Mask4) << 12)
                | ((source[4] & Mask4) << 16)
                | ((source[5] & Mask4) << 20)
                | ((source[6] & Mask4) << 24)
                | (source[7] << 28);
            destination[1] = (source[8] & Mask4)
                | ((source[9] & Mask4) << 4)
                | ((source[10] & Mask4) << 8)
                | ((source[11] & Mask4) << 12)
                | ((source[12] & Mask4) << 16)
                | ((source[13] & Mask4) << 20)
                | ((source[14] & Mask4) << 24)
                | (source[15] << 28);
            destination[2] = (source[16] & Mask4)
                | ((source[17] & Mask4) << 4)
                | ((source[18] & Mask4) << 8)
                | ((source[19] & Mask4) << 12)
                | ((source[20] & Mask4) << 16)
                | ((source[21] & Mask4) << 20)
                | ((source[22] & Mask4) << 24)
                | (source[23] << 28);
            destination[3] = (source[24] & Mask4)
                | ((source[25] & Mask4) << 4)
                | ((source[26] & Mask4) << 8)
                | ((source[27] & Mask4) << 12)
                | ((source[28] & Mask4) << 16)
                | ((source[29] & Mask4) << 20)
                | ((source[30] & Mask4) << 24)
                | (source[31] << 28);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack5(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask5)
                | ((source[1] & Mask5) << 5)
                | ((source[2] & Mask5) << 10)
                | ((source[3] & Mask5) << 15)
                | ((source[4] & Mask5) << 20)
                | ((source[5] & Mask5) << 25)
                | (source[6] << 30);
            destination[1] = (int)(((uint)source[6] & Mask5) >> (5 - 3))
                | ((source[7] & Mask5) << 3)
                | ((source[8] & Mask5) << 8)
                | ((source[9] & Mask5) << 13)
                | ((source[10] & Mask5) << 18)
                | ((source[11] & Mask5) << 23)
                | (source[12] << 28);
            destination[2] = (int)(((uint)source[12] & Mask5) >> (5 - 1))
                | ((source[13] & Mask5) << 1)
                | ((source[14] & Mask5) << 6)
                | ((source[15] & Mask5) << 11)
                | ((source[16] & Mask5) << 16)
                | ((source[17] & Mask5) << 21)
                | ((source[18] & Mask5) << 26)
                | (source[19] << 31);
            destination[3] = (int)(((uint)source[19] & Mask5) >> (5 - 4))
                | ((source[20] & Mask5) << 4)
                | ((source[21] & Mask5) << 9)
                | ((source[22] & Mask5) << 14)
                | ((source[23] & Mask5) << 19)
                | ((source[24] & Mask5) << 24)
                | (source[25] << 29);
            destination[4] = (int)(((uint)source[25] & Mask5) >> (5 - 2))
                | ((source[26] & Mask5) << 2)
                | ((source[27] & Mask5) << 7)
                | ((source[28] & Mask5) << 12)
                | ((source[29] & Mask5) << 17)
                | ((source[30] & Mask5) << 22)
                | (source[31] << 27);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack6(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask6)
                | ((source[1] & Mask6) << 6)
                | ((source[2] & Mask6) << 12)
                | ((source[3] & Mask6) << 18)
                | ((source[4] & Mask6) << 24)
                | (source[5] << 30);
            destination[1] = (int)(((uint)source[5] & Mask6) >> (6 - 4))
                | ((source[6] & Mask6) << 4)
                | ((source[7] & Mask6) << 10)
                | ((source[8] & Mask6) << 16)
                | ((source[9] & Mask6) << 22)
                | (source[10] << 28);
            destination[2] = (int)(((uint)source[10] & Mask6) >> (6 - 2))
                | ((source[11] & Mask6) << 2)
                | ((source[12] & Mask6) << 8)
                | ((source[13] & Mask6) << 14)
                | ((source[14] & Mask6) << 20)
                | (source[15] << 26);
            destination[3] = (source[16] & Mask6)
                | ((source[17] & Mask6) << 6)
                | ((source[18] & Mask6) << 12)
                | ((source[19] & Mask6) << 18)
                | ((source[20] & Mask6) << 24)
                | (source[21] << 30);
            destination[4] = (int)(((uint)source[21] & Mask6) >> (6 - 4))
                | ((source[22] & Mask6) << 4)
                | ((source[23] & Mask6) << 10)
                | ((source[24] & Mask6) << 16)
                | ((source[25] & Mask6) << 22)
                | (source[26] << 28);
            destination[5] = (int)(((uint)source[26] & Mask6) >> (6 - 2))
                | ((source[27] & Mask6) << 2)
                | ((source[28] & Mask6) << 8)
                | ((source[29] & Mask6) << 14)
                | ((source[30] & Mask6) << 20)
                | (source[31] << 26);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack7(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask7)
                | ((source[1] & Mask7) << 7)
                | ((source[2] & Mask7) << 14)
                | ((source[3] & Mask7) << 21)
                | (source[4] << 28);
            destination[1] = (int)(((uint)source[4] & Mask7) >> (7 - 3))
                | ((source[5] & Mask7) << 3)
                | ((source[6] & Mask7) << 10)
                | ((source[7] & Mask7) << 17)
                | ((source[8] & Mask7) << 24)
                | (source[9] << 31);
            destination[2] = (int)(((uint)source[9] & Mask7) >> (7 - 6))
                | ((source[10] & Mask7) << 6)
                | ((source[11] & Mask7) << 13)
                | ((source[12] & Mask7) << 20)
                | (source[13] << 27);
            destination[3] = (int)(((uint)source[13] & Mask7) >> (7 - 2))
                | ((source[14] & Mask7) << 2)
                | ((source[15] & Mask7) << 9)
                | ((source[16] & Mask7) << 16)
                | ((source[17] & Mask7) << 23)
                | (source[18] << 30);
            destination[4] = (int)(((uint)source[18] & Mask7) >> (7 - 5))
                | ((source[19] & Mask7) << 5)
                | ((source[20] & Mask7) << 12)
                | ((source[21] & Mask7) << 19)
                | (source[22] << 26);
            destination[5] = (int)(((uint)source[22] & Mask7) >> (7 - 1))
                | ((source[23] & Mask7) << 1)
                | ((source[24] & Mask7) << 8)
                | ((source[25] & Mask7) << 15)
                | ((source[26] & Mask7) << 22)
                | (source[27] << 29);
            destination[6] = (int)(((uint)source[27] & Mask7) >> (7 - 4))
                | ((source[28] & Mask7) << 4)
                | ((source[29] & Mask7) << 11)
                | ((source[30] & Mask7) << 18)
                | (source[31] << 25);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack8(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask8)
                | ((source[1] & Mask8) << 8)
                | ((source[2] & Mask8) << 16)
                | (source[3] << 24);
            destination[1] = (source[4] & Mask8)
                | ((source[5] & Mask8) << 8)
                | ((source[6] & Mask8) << 16)
                | (source[7] << 24);
            destination[2] = (source[8] & Mask8)
                | ((source[9] & Mask8) << 8)
                | ((source[10] & Mask8) << 16)
                | (source[11] << 24);
            destination[3] = (source[12] & Mask8)
                | ((source[13] & Mask8) << 8)
                | ((source[14] & Mask8) << 16)
                | (source[15] << 24);
            destination[4] = (source[16] & Mask8)
                | ((source[17] & Mask8) << 8)
                | ((source[18] & Mask8) << 16)
                | (source[19] << 24);
            destination[5] = (source[20] & Mask8)
                | ((source[21] & Mask8) << 8)
                | ((source[22] & Mask8) << 16)
                | (source[23] << 24);
            destination[6] = (source[24] & Mask8)
                | ((source[25] & Mask8) << 8)
                | ((source[26] & Mask8) << 16)
                | (source[27] << 24);
            destination[7] = (source[28] & Mask8)
                | ((source[29] & Mask8) << 8)
                | ((source[30] & Mask8) << 16)
                | (source[31] << 24);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack9(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask9)
                | ((source[1] & Mask9) << 9)
                | ((source[2] & Mask9) << 18)
                | (source[3] << 27);
            destination[1] = (int)(((uint)source[3] & Mask9) >> (9 - 4))
                | ((source[4] & Mask9) << 4)
                | ((source[5] & Mask9) << 13)
                | ((source[6] & Mask9) << 22)
                | (source[7] << 31);
            destination[2] = (int)(((uint)source[7] & Mask9) >> (9 - 8))
                | ((source[8] & Mask9) << 8)
                | ((source[9] & Mask9) << 17)
                | (source[10] << 26);
            destination[3] = (int)(((uint)source[10] & Mask9) >> (9 - 3))
                | ((source[11] & Mask9) << 3)
                | ((source[12] & Mask9) << 12)
                | ((source[13] & Mask9) << 21)
                | (source[14] << 30);
            destination[4] = (int)(((uint)source[14] & Mask9) >> (9 - 7))
                | ((source[15] & Mask9) << 7)
                | ((source[16] & Mask9) << 16)
                | (source[17] << 25);
            destination[5] = (int)(((uint)source[17] & Mask9) >> (9 - 2))
                | ((source[18] & Mask9) << 2)
                | ((source[19] & Mask9) << 11)
                | ((source[20] & Mask9) << 20)
                | (source[21] << 29);
            destination[6] = (int)(((uint)source[21] & Mask9) >> (9 - 6))
                | ((source[22] & Mask9) << 6)
                | ((source[23] & Mask9) << 15)
                | (source[24] << 24);
            destination[7] = (int)(((uint)source[24] & Mask9) >> (9 - 1))
                | ((source[25] & Mask9) << 1)
                | ((source[26] & Mask9) << 10)
                | ((source[27] & Mask9) << 19)
                | (source[28] << 28);
            destination[8] = (int)(((uint)source[28] & Mask9) >> (9 - 5))
                | ((source[29] & Mask9) << 5)
                | ((source[30] & Mask9) << 14)
                | (source[31] << 23);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack10(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask10)
                | ((source[1] & Mask10) << 10)
                | ((source[2] & Mask10) << 20)
                | (source[3] << 30);
            destination[1] = (int)(((uint)source[3] & Mask10) >> (10 - 8))
                | ((source[4] & Mask10) << 8)
                | ((source[5] & Mask10) << 18)
                | (source[6] << 28);
            destination[2] = (int)(((uint)source[6] & Mask10) >> (10 - 6))
                | ((source[7] & Mask10) << 6)
                | ((source[8] & Mask10) << 16)
                | (source[9] << 26);
            destination[3] = (int)(((uint)source[9] & Mask10) >> (10 - 4))
                | ((source[10] & Mask10) << 4)
                | ((source[11] & Mask10) << 14)
                | (source[12] << 24);
            destination[4] = (int)(((uint)source[12] & Mask10) >> (10 - 2))
                | ((source[13] & Mask10) << 2)
                | ((source[14] & Mask10) << 12)
                | (source[15] << 22);
            destination[5] = (source[16] & Mask10)
                | ((source[17] & Mask10) << 10)
                | ((source[18] & Mask10) << 20)
                | (source[19] << 30);
            destination[6] = (int)(((uint)source[19] & Mask10) >> (10 - 8))
                | ((source[20] & Mask10) << 8)
                | ((source[21] & Mask10) << 18)
                | (source[22] << 28);
            destination[7] = (int)(((uint)source[22] & Mask10) >> (10 - 6))
                | ((source[23] & Mask10) << 6)
                | ((source[24] & Mask10) << 16)
                | (source[25] << 26);
            destination[8] = (int)(((uint)source[25] & Mask10) >> (10 - 4))
                | ((source[26] & Mask10) << 4)
                | ((source[27] & Mask10) << 14)
                | (source[28] << 24);
            destination[9] = (int)(((uint)source[28] & Mask10) >> (10 - 2))
                | ((source[29] & Mask10) << 2)
                | ((source[30] & Mask10) << 12)
                | (source[31] << 22);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack11(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask11)
                | ((source[1] & Mask11) << 11)
                | (source[2] << 22);
            destination[1] = (int)(((uint)source[2] & Mask11) >> (11 - 1))
                | ((source[3] & Mask11) << 1)
                | ((source[4] & Mask11) << 12)
                | (source[5] << 23);
            destination[2] = (int)(((uint)source[5] & Mask11) >> (11 - 2))
                | ((source[6] & Mask11) << 2)
                | ((source[7] & Mask11) << 13)
                | (source[8] << 24);
            destination[3] = (int)(((uint)source[8] & Mask11) >> (11 - 3))
                | ((source[9] & Mask11) << 3)
                | ((source[10] & Mask11) << 14)
                | (source[11] << 25);
            destination[4] = (int)(((uint)source[11] & Mask11) >> (11 - 4))
                | ((source[12] & Mask11) << 4)
                | ((source[13] & Mask11) << 15)
                | (source[14] << 26);
            destination[5] = (int)(((uint)source[14] & Mask11) >> (11 - 5))
                | ((source[15] & Mask11) << 5)
                | ((source[16] & Mask11) << 16)
                | (source[17] << 27);
            destination[6] = (int)(((uint)source[17] & Mask11) >> (11 - 6))
                | ((source[18] & Mask11) << 6)
                | ((source[19] & Mask11) << 17)
                | (source[20] << 28);
            destination[7] = (int)(((uint)source[20] & Mask11) >> (11 - 7))
                | ((source[21] & Mask11) << 7)
                | ((source[22] & Mask11) << 18)
                | (source[23] << 29);
            destination[8] = (int)(((uint)source[23] & Mask11) >> (11 - 8))
                | ((source[24] & Mask11) << 8)
                | ((source[25] & Mask11) << 19)
                | (source[26] << 30);
            destination[9] = (int)(((uint)source[26] & Mask11) >> (11 - 9))
                | ((source[27] & Mask11) << 9)
                | ((source[28] & Mask11) << 20)
                | (source[29] << 31);
            destination[10] = (int)(((uint)source[29] & Mask11) >> (11 - 10))
                | ((source[30] & Mask11) << 10)
                | (source[31] << 21);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack12(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask12)
                | ((source[1] & Mask12) << 12)
                | (source[2] << 24);
            destination[1] = (int)(((uint)source[2] & Mask12) >> (12 - 4))
                | ((source[3] & Mask12) << 4)
                | ((source[4] & Mask12) << 16)
                | (source[5] << 28);
            destination[2] = (int)(((uint)source[5] & Mask12) >> (12 - 8))
                | ((source[6] & Mask12) << 8)
                | (source[7] << 20);
            destination[3] = (source[8] & Mask12)
                | ((source[9] & Mask12) << 12)
                | (source[10] << 24);
            destination[4] = (int)(((uint)source[10] & Mask12) >> (12 - 4))
                | ((source[11] & Mask12) << 4)
                | ((source[12] & Mask12) << 16)
                | (source[13] << 28);
            destination[5] = (int)(((uint)source[13] & Mask12) >> (12 - 8))
                | ((source[14] & Mask12) << 8)
                | (source[15] << 20);
            destination[6] = (source[16] & Mask12)
                | ((source[17] & Mask12) << 12)
                | (source[18] << 24);
            destination[7] = (int)(((uint)source[18] & Mask12) >> (12 - 4))
                | ((source[19] & Mask12) << 4)
                | ((source[20] & Mask12) << 16)
                | (source[21] << 28);
            destination[8] = (int)(((uint)source[21] & Mask12) >> (12 - 8))
                | ((source[22] & Mask12) << 8)
                | (source[23] << 20);
            destination[9] = (source[24] & Mask12)
                | ((source[25] & Mask12) << 12)
                | (source[26] << 24);
            destination[10] = (int)(((uint)source[26] & Mask12) >> (12 - 4))
                | ((source[27] & Mask12) << 4)
                | ((source[28] & Mask12) << 16)
                | (source[29] << 28);
            destination[11] = (int)(((uint)source[29] & Mask12) >> (12 - 8))
                | ((source[30] & Mask12) << 8)
                | (source[31] << 20);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack13(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask13)
                | ((source[1] & Mask13) << 13)
                | (source[2] << 26);
            destination[1] = (int)(((uint)source[2] & Mask13) >> (13 - 7))
                | ((source[3] & Mask13) << 7)
                | (source[4] << 20);
            destination[2] = (int)(((uint)source[4] & Mask13) >> (13 - 1))
                | ((source[5] & Mask13) << 1)
                | ((source[6] & Mask13) << 14)
                | (source[7] << 27);
            destination[3] = (int)(((uint)source[7] & Mask13) >> (13 - 8))
                | ((source[8] & Mask13) << 8)
                | (source[9] << 21);
            destination[4] = (int)(((uint)source[9] & Mask13) >> (13 - 2))
                | ((source[10] & Mask13) << 2)
                | ((source[11] & Mask13) << 15)
                | (source[12] << 28);
            destination[5] = (int)(((uint)source[12] & Mask13) >> (13 - 9))
                | ((source[13] & Mask13) << 9)
                | (source[14] << 22);
            destination[6] = (int)(((uint)source[14] & Mask13) >> (13 - 3))
                | ((source[15] & Mask13) << 3)
                | ((source[16] & Mask13) << 16)
                | (source[17] << 29);
            destination[7] = (int)(((uint)source[17] & Mask13) >> (13 - 10))
                | ((source[18] & Mask13) << 10)
                | (source[19] << 23);
            destination[8] = (int)(((uint)source[19] & Mask13) >> (13 - 4))
                | ((source[20] & Mask13) << 4)
                | ((source[21] & Mask13) << 17)
                | (source[22] << 30);
            destination[9] = (int)(((uint)source[22] & Mask13) >> (13 - 11))
                | ((source[23] & Mask13) << 11)
                | (source[24] << 24);
            destination[10] = (int)(((uint)source[24] & Mask13) >> (13 - 5))
                | ((source[25] & Mask13) << 5)
                | ((source[26] & Mask13) << 18)
                | (source[27] << 31);
            destination[11] = (int)(((uint)source[27] & Mask13) >> (13 - 12))
                | ((source[28] & Mask13) << 12)
                | (source[29] << 25);
            destination[12] = (int)(((uint)source[29] & Mask13) >> (13 - 6))
                | ((source[30] & Mask13) << 6)
                | (source[31] << 19);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack14(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask14)
                | ((source[1] & Mask14) << 14)
                | (source[2] << 28);
            destination[1] = (int)(((uint)source[2] & Mask14) >> (14 - 10))
                | ((source[3] & Mask14) << 10)
                | (source[4] << 24);
            destination[2] = (int)(((uint)source[4] & Mask14) >> (14 - 6))
                | ((source[5] & Mask14) << 6)
                | (source[6] << 20);
            destination[3] = (int)(((uint)source[6] & Mask14) >> (14 - 2))
                | ((source[7] & Mask14) << 2)
                | ((source[8] & Mask14) << 16)
                | (source[9] << 30);
            destination[4] = (int)(((uint)source[9] & Mask14) >> (14 - 12))
                | ((source[10] & Mask14) << 12)
                | (source[11] << 26);
            destination[5] = (int)(((uint)source[11] & Mask14) >> (14 - 8))
                | ((source[12] & Mask14) << 8)
                | (source[13] << 22);
            destination[6] = (int)(((uint)source[13] & Mask14) >> (14 - 4))
                | ((source[14] & Mask14) << 4)
                | (source[15] << 18);
            destination[7] = (source[16] & Mask14)
                | ((source[17] & Mask14) << 14)
                | (source[18] << 28);
            destination[8] = (int)(((uint)source[18] & Mask14) >> (14 - 10))
                | ((source[19] & Mask14) << 10)
                | (source[20] << 24);
            destination[9] = (int)(((uint)source[20] & Mask14) >> (14 - 6))
                | ((source[21] & Mask14) << 6)
                | (source[22] << 20);
            destination[10] = (int)(((uint)source[22] & Mask14) >> (14 - 2))
                | ((source[23] & Mask14) << 2)
                | ((source[24] & Mask14) << 16)
                | (source[25] << 30);
            destination[11] = (int)(((uint)source[25] & Mask14) >> (14 - 12))
                | ((source[26] & Mask14) << 12)
                | (source[27] << 26);
            destination[12] = (int)(((uint)source[27] & Mask14) >> (14 - 8))
                | ((source[28] & Mask14) << 8)
                | (source[29] << 22);
            destination[13] = (int)(((uint)source[29] & Mask14) >> (14 - 4))
                | ((source[30] & Mask14) << 4)
                | (source[31] << 18);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack15(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask15)
                | ((source[1] & Mask15) << 15)
                | (source[2] << 30);
            destination[1] = (int)(((uint)source[2] & Mask15) >> (15 - 13))
                | ((source[3] & Mask15) << 13)
                | (source[4] << 28);
            destination[2] = (int)(((uint)source[4] & Mask15) >> (15 - 11))
                | ((source[5] & Mask15) << 11)
                | (source[6] << 26);
            destination[3] = (int)(((uint)source[6] & Mask15) >> (15 - 9))
                | ((source[7] & Mask15) << 9)
                | (source[8] << 24);
            destination[4] = (int)(((uint)source[8] & Mask15) >> (15 - 7))
                | ((source[9] & Mask15) << 7)
                | (source[10] << 22);
            destination[5] = (int)(((uint)source[10] & Mask15) >> (15 - 5))
                | ((source[11] & Mask15) << 5)
                | (source[12] << 20);
            destination[6] = (int)(((uint)source[12] & Mask15) >> (15 - 3))
                | ((source[13] & Mask15) << 3)
                | (source[14] << 18);
            destination[7] = (int)(((uint)source[14] & Mask15) >> (15 - 1))
                | ((source[15] & Mask15) << 1)
                | ((source[16] & Mask15) << 16)
                | (source[17] << 31);
            destination[8] = (int)(((uint)source[17] & Mask15) >> (15 - 14))
                | ((source[18] & Mask15) << 14)
                | (source[19] << 29);
            destination[9] = (int)(((uint)source[19] & Mask15) >> (15 - 12))
                | ((source[20] & Mask15) << 12)
                | (source[21] << 27);
            destination[10] = (int)(((uint)source[21] & Mask15) >> (15 - 10))
                | ((source[22] & Mask15) << 10)
                | (source[23] << 25);
            destination[11] = (int)(((uint)source[23] & Mask15) >> (15 - 8))
                | ((source[24] & Mask15) << 8)
                | (source[25] << 23);
            destination[12] = (int)(((uint)source[25] & Mask15) >> (15 - 6))
                | ((source[26] & Mask15) << 6)
                | (source[27] << 21);
            destination[13] = (int)(((uint)source[27] & Mask15) >> (15 - 4))
                | ((source[28] & Mask15) << 4)
                | (source[29] << 19);
            destination[14] = (int)(((uint)source[29] & Mask15) >> (15 - 2))
                | ((source[30] & Mask15) << 2)
                | (source[31] << 17);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack16(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask16) | (source[1] << 16);
            destination[1] = (source[2] & Mask16) | (source[3] << 16);
            destination[2] = (source[4] & Mask16) | (source[5] << 16);
            destination[3] = (source[6] & Mask16) | (source[7] << 16);
            destination[4] = (source[8] & Mask16) | (source[9] << 16);
            destination[5] = (source[10] & Mask16) | (source[11] << 16);
            destination[6] = (source[12] & Mask16) | (source[13] << 16);
            destination[7] = (source[14] & Mask16) | (source[15] << 16);
            destination[8] = (source[16] & Mask16) | (source[17] << 16);
            destination[9] = (source[18] & Mask16) | (source[19] << 16);
            destination[10] = (source[20] & Mask16) | (source[21] << 16);
            destination[11] = (source[22] & Mask16) | (source[23] << 16);
            destination[12] = (source[24] & Mask16) | (source[25] << 16);
            destination[13] = (source[26] & Mask16) | (source[27] << 16);
            destination[14] = (source[28] & Mask16) | (source[29] << 16);
            destination[15] = (source[30] & Mask16) | (source[31] << 16);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack17(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask17) | (source[1] << 17);
            destination[1] = (int)(((uint)source[1] & Mask17) >> (17 - 2))
                | ((source[2] & Mask17) << 2)
                | (source[3] << 19);
            destination[2] = (int)(((uint)source[3] & Mask17) >> (17 - 4))
                | ((source[4] & Mask17) << 4)
                | (source[5] << 21);
            destination[3] = (int)(((uint)source[5] & Mask17) >> (17 - 6))
                | ((source[6] & Mask17) << 6)
                | (source[7] << 23);
            destination[4] = (int)(((uint)source[7] & Mask17) >> (17 - 8))
                | ((source[8] & Mask17) << 8)
                | (source[9] << 25);
            destination[5] = (int)(((uint)source[9] & Mask17) >> (17 - 10))
                | ((source[10] & Mask17) << 10)
                | (source[11] << 27);
            destination[6] = (int)(((uint)source[11] & Mask17) >> (17 - 12))
                | ((source[12] & Mask17) << 12)
                | (source[13] << 29);
            destination[7] = (int)(((uint)source[13] & Mask17) >> (17 - 14))
                | ((source[14] & Mask17) << 14)
                | (source[15] << 31);
            destination[8] = (int)(((uint)source[15] & Mask17) >> (17 - 16)) | (source[16] << 16);
            destination[9] = (int)(((uint)source[16] & Mask17) >> (17 - 1))
                | ((source[17] & Mask17) << 1)
                | (source[18] << 18);
            destination[10] = (int)(((uint)source[18] & Mask17) >> (17 - 3))
                | ((source[19] & Mask17) << 3)
                | (source[20] << 20);
            destination[11] = (int)(((uint)source[20] & Mask17) >> (17 - 5))
                | ((source[21] & Mask17) << 5)
                | (source[22] << 22);
            destination[12] = (int)(((uint)source[22] & Mask17) >> (17 - 7))
                | ((source[23] & Mask17) << 7)
                | (source[24] << 24);
            destination[13] = (int)(((uint)source[24] & Mask17) >> (17 - 9))
                | ((source[25] & Mask17) << 9)
                | (source[26] << 26);
            destination[14] = (int)(((uint)source[26] & Mask17) >> (17 - 11))
                | ((source[27] & Mask17) << 11)
                | (source[28] << 28);
            destination[15] = (int)(((uint)source[28] & Mask17) >> (17 - 13))
                | ((source[29] & Mask17) << 13)
                | (source[30] << 30);
            destination[16] = (int)(((uint)source[30] & Mask17) >> (17 - 15)) | (source[31] << 15);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack18(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask18) | (source[1] << 18);
            destination[1] = (int)(((uint)source[1] & Mask18) >> (18 - 4))
                | ((source[2] & Mask18) << 4)
                | (source[3] << 22);
            destination[2] = (int)(((uint)source[3] & Mask18) >> (18 - 8))
                | ((source[4] & Mask18) << 8)
                | (source[5] << 26);
            destination[3] = (int)(((uint)source[5] & Mask18) >> (18 - 12))
                | ((source[6] & Mask18) << 12)
                | (source[7] << 30);
            destination[4] = (int)(((uint)source[7] & Mask18) >> (18 - 16)) | (source[8] << 16);
            destination[5] = (int)(((uint)source[8] & Mask18) >> (18 - 2))
                | ((source[9] & Mask18) << 2)
                | (source[10] << 20);
            destination[6] = (int)(((uint)source[10] & Mask18) >> (18 - 6))
                | ((source[11] & Mask18) << 6)
                | (source[12] << 24);
            destination[7] = (int)(((uint)source[12] & Mask18) >> (18 - 10))
                | ((source[13] & Mask18) << 10)
                | (source[14] << 28);
            destination[8] = (int)(((uint)source[14] & Mask18) >> (18 - 14)) | (source[15] << 14);
            destination[9] = (source[16] & Mask18) | (source[17] << 18);
            destination[10] = (int)(((uint)source[17] & Mask18) >> (18 - 4))
                | ((source[18] & Mask18) << 4)
                | (source[19] << 22);
            destination[11] = (int)(((uint)source[19] & Mask18) >> (18 - 8))
                | ((source[20] & Mask18) << 8)
                | (source[21] << 26);
            destination[12] = (int)(((uint)source[21] & Mask18) >> (18 - 12))
                | ((source[22] & Mask18) << 12)
                | (source[23] << 30);
            destination[13] = (int)(((uint)source[23] & Mask18) >> (18 - 16)) | (source[24] << 16);
            destination[14] = (int)(((uint)source[24] & Mask18) >> (18 - 2))
                | ((source[25] & Mask18) << 2)
                | (source[26] << 20);
            destination[15] = (int)(((uint)source[26] & Mask18) >> (18 - 6))
                | ((source[27] & Mask18) << 6)
                | (source[28] << 24);
            destination[16] = (int)(((uint)source[28] & Mask18) >> (18 - 10))
                | ((source[29] & Mask18) << 10)
                | (source[30] << 28);
            destination[17] = (int)(((uint)source[30] & Mask18) >> (18 - 14)) | (source[31] << 14);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack19(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask19) | (source[1] << 19);
            destination[1] = (int)(((uint)source[1] & Mask19) >> (19 - 6))
                | ((source[2] & Mask19) << 6)
                | (source[3] << 25);
            destination[2] = (int)(((uint)source[3] & Mask19) >> (19 - 12))
                | ((source[4] & Mask19) << 12)
                | (source[5] << 31);
            destination[3] = (int)(((uint)source[5] & Mask19) >> (19 - 18)) | (source[6] << 18);
            destination[4] = (int)(((uint)source[6] & Mask19) >> (19 - 5))
                | ((source[7] & Mask19) << 5)
                | (source[8] << 24);
            destination[5] = (int)(((uint)source[8] & Mask19) >> (19 - 11))
                | ((source[9] & Mask19) << 11)
                | (source[10] << 30);
            destination[6] = (int)(((uint)source[10] & Mask19) >> (19 - 17)) | (source[11] << 17);
            destination[7] = (int)(((uint)source[11] & Mask19) >> (19 - 4))
                | ((source[12] & Mask19) << 4)
                | (source[13] << 23);
            destination[8] = (int)(((uint)source[13] & Mask19) >> (19 - 10))
                | ((source[14] & Mask19) << 10)
                | (source[15] << 29);
            destination[9] = (int)(((uint)source[15] & Mask19) >> (19 - 16)) | (source[16] << 16);
            destination[10] = (int)(((uint)source[16] & Mask19) >> (19 - 3))
                | ((source[17] & Mask19) << 3)
                | (source[18] << 22);
            destination[11] = (int)(((uint)source[18] & Mask19) >> (19 - 9))
                | ((source[19] & Mask19) << 9)
                | (source[20] << 28);
            destination[12] = (int)(((uint)source[20] & Mask19) >> (19 - 15)) | (source[21] << 15);
            destination[13] = (int)(((uint)source[21] & Mask19) >> (19 - 2))
                | ((source[22] & Mask19) << 2)
                | (source[23] << 21);
            destination[14] = (int)(((uint)source[23] & Mask19) >> (19 - 8))
                | ((source[24] & Mask19) << 8)
                | (source[25] << 27);
            destination[15] = (int)(((uint)source[25] & Mask19) >> (19 - 14)) | (source[26] << 14);
            destination[16] = (int)(((uint)source[26] & Mask19) >> (19 - 1))
                | ((source[27] & Mask19) << 1)
                | (source[28] << 20);
            destination[17] = (int)(((uint)source[28] & Mask19) >> (19 - 7))
                | ((source[29] & Mask19) << 7)
                | (source[30] << 26);
            destination[18] = (int)(((uint)source[30] & Mask19) >> (19 - 13)) | (source[31] << 13);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack20(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask20) | (source[1] << 20);
            destination[1] = (int)(((uint)source[1] & Mask20) >> (20 - 8))
                | ((source[2] & Mask20) << 8)
                | (source[3] << 28);
            destination[2] = (int)(((uint)source[3] & Mask20) >> (20 - 16)) | (source[4] << 16);
            destination[3] = (int)(((uint)source[4] & Mask20) >> (20 - 4))
                | ((source[5] & Mask20) << 4)
                | (source[6] << 24);
            destination[4] = (int)(((uint)source[6] & Mask20) >> (20 - 12)) | (source[7] << 12);
            destination[5] = (source[8] & Mask20) | (source[9] << 20);
            destination[6] = (int)(((uint)source[9] & Mask20) >> (20 - 8))
                | ((source[10] & Mask20) << 8)
                | (source[11] << 28);
            destination[7] = (int)(((uint)source[11] & Mask20) >> (20 - 16)) | (source[12] << 16);
            destination[8] = (int)(((uint)source[12] & Mask20) >> (20 - 4))
                | ((source[13] & Mask20) << 4)
                | (source[14] << 24);
            destination[9] = (int)(((uint)source[14] & Mask20) >> (20 - 12)) | (source[15] << 12);
            destination[10] = (source[16] & Mask20) | (source[17] << 20);
            destination[11] = (int)(((uint)source[17] & Mask20) >> (20 - 8))
                | ((source[18] & Mask20) << 8)
                | (source[19] << 28);
            destination[12] = (int)(((uint)source[19] & Mask20) >> (20 - 16)) | (source[20] << 16);
            destination[13] = (int)(((uint)source[20] & Mask20) >> (20 - 4))
                | ((source[21] & Mask20) << 4)
                | (source[22] << 24);
            destination[14] = (int)(((uint)source[22] & Mask20) >> (20 - 12)) | (source[23] << 12);
            destination[15] = (source[24] & Mask20) | (source[25] << 20);
            destination[16] = (int)(((uint)source[25] & Mask20) >> (20 - 8))
                | ((source[26] & Mask20) << 8)
                | (source[27] << 28);
            destination[17] = (int)(((uint)source[27] & Mask20) >> (20 - 16)) | (source[28] << 16);
            destination[18] = (int)(((uint)source[28] & Mask20) >> (20 - 4))
                | ((source[29] & Mask20) << 4)
                | (source[30] << 24);
            destination[19] = (int)(((uint)source[30] & Mask20) >> (20 - 12)) | (source[31] << 12);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack21(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask21) | (source[1] << 21);
            destination[1] = (int)(((uint)source[1] & Mask21) >> (21 - 10))
                | ((source[2] & Mask21) << 10)
                | (source[3] << 31);
            destination[2] = (int)(((uint)source[3] & Mask21) >> (21 - 20)) | (source[4] << 20);
            destination[3] = (int)(((uint)source[4] & Mask21) >> (21 - 9))
                | ((source[5] & Mask21) << 9)
                | (source[6] << 30);
            destination[4] = (int)(((uint)source[6] & Mask21) >> (21 - 19)) | (source[7] << 19);
            destination[5] = (int)(((uint)source[7] & Mask21) >> (21 - 8))
                | ((source[8] & Mask21) << 8)
                | (source[9] << 29);
            destination[6] = (int)(((uint)source[9] & Mask21) >> (21 - 18)) | (source[10] << 18);
            destination[7] = (int)(((uint)source[10] & Mask21) >> (21 - 7))
                | ((source[11] & Mask21) << 7)
                | (source[12] << 28);
            destination[8] = (int)(((uint)source[12] & Mask21) >> (21 - 17)) | (source[13] << 17);
            destination[9] = (int)(((uint)source[13] & Mask21) >> (21 - 6))
                | ((source[14] & Mask21) << 6)
                | (source[15] << 27);
            destination[10] = (int)(((uint)source[15] & Mask21) >> (21 - 16)) | (source[16] << 16);
            destination[11] = (int)(((uint)source[16] & Mask21) >> (21 - 5))
                | ((source[17] & Mask21) << 5)
                | (source[18] << 26);
            destination[12] = (int)(((uint)source[18] & Mask21) >> (21 - 15)) | (source[19] << 15);
            destination[13] = (int)(((uint)source[19] & Mask21) >> (21 - 4))
                | ((source[20] & Mask21) << 4)
                | (source[21] << 25);
            destination[14] = (int)(((uint)source[21] & Mask21) >> (21 - 14)) | (source[22] << 14);
            destination[15] = (int)(((uint)source[22] & Mask21) >> (21 - 3))
                | ((source[23] & Mask21) << 3)
                | (source[24] << 24);
            destination[16] = (int)(((uint)source[24] & Mask21) >> (21 - 13)) | (source[25] << 13);
            destination[17] = (int)(((uint)source[25] & Mask21) >> (21 - 2))
                | ((source[26] & Mask21) << 2)
                | (source[27] << 23);
            destination[18] = (int)(((uint)source[27] & Mask21) >> (21 - 12)) | (source[28] << 12);
            destination[19] = (int)(((uint)source[28] & Mask21) >> (21 - 1))
                | ((source[29] & Mask21) << 1)
                | (source[30] << 22);
            destination[20] = (int)(((uint)source[30] & Mask21) >> (21 - 11)) | (source[31] << 11);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack22(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask22) | (source[1] << 22);
            destination[1] = (int)(((uint)source[1] & Mask22) >> (22 - 12)) | (source[2] << 12);
            destination[2] = (int)(((uint)source[2] & Mask22) >> (22 - 2)) | ((source[3] & Mask22) << 2) | (source[4] << 24);
            destination[3] = (int)(((uint)source[4] & Mask22) >> (22 - 14)) | (source[5] << 14);
            destination[4] = (int)(((uint)source[5] & Mask22) >> (22 - 4)) | ((source[6] & Mask22) << 4) | (source[7] << 26);
            destination[5] = (int)(((uint)source[7] & Mask22) >> (22 - 16)) | (source[8] << 16);
            destination[6] = (int)(((uint)source[8] & Mask22) >> (22 - 6)) | ((source[9] & Mask22) << 6) | (source[10] << 28);
            destination[7] = (int)(((uint)source[10] & Mask22) >> (22 - 18)) | (source[11] << 18);
            destination[8] = (int)(((uint)source[11] & Mask22) >> (22 - 8)) | ((source[12] & Mask22) << 8) | (source[13] << 30);
            destination[9] = (int)(((uint)source[13] & Mask22) >> (22 - 20)) | (source[14] << 20);
            destination[10] = (int)(((uint)source[14] & Mask22) >> (22 - 10)) | (source[15] << 10);
            destination[11] = (source[16] & Mask22) | (source[17] << 22);
            destination[12] = (int)(((uint)source[17] & Mask22) >> (22 - 12)) | (source[18] << 12);
            destination[13] = (int)(((uint)source[18] & Mask22) >> (22 - 2)) | ((source[19] & Mask22) << 2) | (source[20] << 24);
            destination[14] = (int)(((uint)source[20] & Mask22) >> (22 - 14)) | (source[21] << 14);
            destination[15] = (int)(((uint)source[21] & Mask22) >> (22 - 4)) | ((source[22] & Mask22) << 4) | (source[23] << 26);
            destination[16] = (int)(((uint)source[23] & Mask22) >> (22 - 16)) | (source[24] << 16);
            destination[17] = (int)(((uint)source[24] & Mask22) >> (22 - 6)) | ((source[25] & Mask22) << 6) | (source[26] << 28);
            destination[18] = (int)(((uint)source[26] & Mask22) >> (22 - 18)) | (source[27] << 18);
            destination[19] = (int)(((uint)source[27] & Mask22) >> (22 - 8)) | ((source[28] & Mask22) << 8) | (source[29] << 30);
            destination[20] = (int)(((uint)source[29] & Mask22) >> (22 - 20)) | (source[30] << 20);
            destination[21] = (int)(((uint)source[30] & Mask22) >> (22 - 10)) | (source[31] << 10);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack23(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask23) | (source[1] << 23);
            destination[1] = (int)(((uint)source[1] & Mask23) >> (23 - 14)) | (source[2] << 14);
            destination[2] = (int)(((uint)source[2] & Mask23) >> (23 - 5)) | ((source[3] & Mask23) << 5) | (source[4] << 28);
            destination[3] = (int)(((uint)source[4] & Mask23) >> (23 - 19)) | (source[5] << 19);
            destination[4] = (int)(((uint)source[5] & Mask23) >> (23 - 10)) | (source[6] << 10);
            destination[5] = (int)(((uint)source[6] & Mask23) >> (23 - 1)) | ((source[7] & Mask23) << 1) | (source[8] << 24);
            destination[6] = (int)(((uint)source[8] & Mask23) >> (23 - 15)) | (source[9] << 15);
            destination[7] = (int)(((uint)source[9] & Mask23) >> (23 - 6)) | ((source[10] & Mask23) << 6) | (source[11] << 29);
            destination[8] = (int)(((uint)source[11] & Mask23) >> (23 - 20)) | (source[12] << 20);
            destination[9] = (int)(((uint)source[12] & Mask23) >> (23 - 11)) | (source[13] << 11);
            destination[10] = (int)(((uint)source[13] & Mask23) >> (23 - 2)) | ((source[14] & Mask23) << 2) | (source[15] << 25);
            destination[11] = (int)(((uint)source[15] & Mask23) >> (23 - 16)) | (source[16] << 16);
            destination[12] = (int)(((uint)source[16] & Mask23) >> (23 - 7)) | ((source[17] & Mask23) << 7) | (source[18] << 30);
            destination[13] = (int)(((uint)source[18] & Mask23) >> (23 - 21)) | (source[19] << 21);
            destination[14] = (int)(((uint)source[19] & Mask23) >> (23 - 12)) | (source[20] << 12);
            destination[15] = (int)(((uint)source[20] & Mask23) >> (23 - 3)) | ((source[21] & Mask23) << 3) | (source[22] << 26);
            destination[16] = (int)(((uint)source[22] & Mask23) >> (23 - 17)) | (source[23] << 17);
            destination[17] = (int)(((uint)source[23] & Mask23) >> (23 - 8)) | ((source[24] & Mask23) << 8) | (source[25] << 31);
            destination[18] = (int)(((uint)source[25] & Mask23) >> (23 - 22)) | (source[26] << 22);
            destination[19] = (int)(((uint)source[26] & Mask23) >> (23 - 13)) | (source[27] << 13);
            destination[20] = (int)(((uint)source[27] & Mask23) >> (23 - 4)) | ((source[28] & Mask23) << 4) | (source[29] << 27);
            destination[21] = (int)(((uint)source[29] & Mask23) >> (23 - 18)) | (source[30] << 18);
            destination[22] = (int)(((uint)source[30] & Mask23) >> (23 - 9)) | (source[31] << 9);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack24(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask24) | (source[1] << 24);
            destination[1] = (int)(((uint)source[1] & Mask24) >> (24 - 16)) | (source[2] << 16);
            destination[2] = (int)(((uint)source[2] & Mask24) >> (24 - 8)) | (source[3] << 8);
            destination[3] = (source[4] & Mask24) | (source[5] << 24);
            destination[4] = (int)(((uint)source[5] & Mask24) >> (24 - 16)) | (source[6] << 16);
            destination[5] = (int)(((uint)source[6] & Mask24) >> (24 - 8)) | (source[7] << 8);
            destination[6] = (source[8] & Mask24) | (source[9] << 24);
            destination[7] = (int)(((uint)source[9] & Mask24) >> (24 - 16)) | (source[10] << 16);
            destination[8] = (int)(((uint)source[10] & Mask24) >> (24 - 8)) | (source[11] << 8);
            destination[9] = (source[12] & Mask24) | (source[13] << 24);
            destination[10] = (int)(((uint)source[13] & Mask24) >> (24 - 16)) | (source[14] << 16);
            destination[11] = (int)(((uint)source[14] & Mask24) >> (24 - 8)) | (source[15] << 8);
            destination[12] = (source[16] & Mask24) | (source[17] << 24);
            destination[13] = (int)(((uint)source[17] & Mask24) >> (24 - 16)) | (source[18] << 16);
            destination[14] = (int)(((uint)source[18] & Mask24) >> (24 - 8)) | (source[19] << 8);
            destination[15] = (source[20] & Mask24) | (source[21] << 24);
            destination[16] = (int)(((uint)source[21] & Mask24) >> (24 - 16)) | (source[22] << 16);
            destination[17] = (int)(((uint)source[22] & Mask24) >> (24 - 8)) | (source[23] << 8);
            destination[18] = (source[24] & Mask24) | (source[25] << 24);
            destination[19] = (int)(((uint)source[25] & Mask24) >> (24 - 16)) | (source[26] << 16);
            destination[20] = (int)(((uint)source[26] & Mask24) >> (24 - 8)) | (source[27] << 8);
            destination[21] = (source[28] & Mask24) | (source[29] << 24);
            destination[22] = (int)(((uint)source[29] & Mask24) >> (24 - 16)) | (source[30] << 16);
            destination[23] = (int)(((uint)source[30] & Mask24) >> (24 - 8)) | (source[31] << 8);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack25(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask25) | (source[1] << 25);
            destination[1] = (int)(((uint)source[1] & Mask25) >> (25 - 18)) | (source[2] << 18);
            destination[2] = (int)(((uint)source[2] & Mask25) >> (25 - 11)) | (source[3] << 11);
            destination[3] = (int)(((uint)source[3] & Mask25) >> (25 - 4)) | ((source[4] & Mask25) << 4) | (source[5] << 29);
            destination[4] = (int)(((uint)source[5] & Mask25) >> (25 - 22)) | (source[6] << 22);
            destination[5] = (int)(((uint)source[6] & Mask25) >> (25 - 15)) | (source[7] << 15);
            destination[6] = (int)(((uint)source[7] & Mask25) >> (25 - 8)) | (source[8] << 8);
            destination[7] = (int)(((uint)source[8] & Mask25) >> (25 - 1)) | ((source[9] & Mask25) << 1) | (source[10] << 26);
            destination[8] = (int)(((uint)source[10] & Mask25) >> (25 - 19)) | (source[11] << 19);
            destination[9] = (int)(((uint)source[11] & Mask25) >> (25 - 12)) | (source[12] << 12);
            destination[10] = (int)(((uint)source[12] & Mask25) >> (25 - 5)) | ((source[13] & Mask25) << 5) | (source[14] << 30);
            destination[11] = (int)(((uint)source[14] & Mask25) >> (25 - 23)) | (source[15] << 23);
            destination[12] = (int)(((uint)source[15] & Mask25) >> (25 - 16)) | (source[16] << 16);
            destination[13] = (int)(((uint)source[16] & Mask25) >> (25 - 9)) | (source[17] << 9);
            destination[14] = (int)(((uint)source[17] & Mask25) >> (25 - 2)) | ((source[18] & Mask25) << 2) | (source[19] << 27);
            destination[15] = (int)(((uint)source[19] & Mask25) >> (25 - 20)) | (source[20] << 20);
            destination[16] = (int)(((uint)source[20] & Mask25) >> (25 - 13)) | (source[21] << 13);
            destination[17] = (int)(((uint)source[21] & Mask25) >> (25 - 6)) | ((source[22] & Mask25) << 6) | (source[23] << 31);
            destination[18] = (int)(((uint)source[23] & Mask25) >> (25 - 24)) | (source[24] << 24);
            destination[19] = (int)(((uint)source[24] & Mask25) >> (25 - 17)) | (source[25] << 17);
            destination[20] = (int)(((uint)source[25] & Mask25) >> (25 - 10)) | (source[26] << 10);
            destination[21] = (int)(((uint)source[26] & Mask25) >> (25 - 3)) | ((source[27] & Mask25) << 3) | (source[28] << 28);
            destination[22] = (int)(((uint)source[28] & Mask25) >> (25 - 21)) | (source[29] << 21);
            destination[23] = (int)(((uint)source[29] & Mask25) >> (25 - 14)) | (source[30] << 14);
            destination[24] = (int)(((uint)source[30] & Mask25) >> (25 - 7)) | (source[31] << 7);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack26(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask26) | (source[1] << 26);
            destination[1] = (int)(((uint)source[1] & Mask26) >> (26 - 20)) | (source[2] << 20);
            destination[2] = (int)(((uint)source[2] & Mask26) >> (26 - 14)) | (source[3] << 14);
            destination[3] = (int)(((uint)source[3] & Mask26) >> (26 - 8)) | (source[4] << 8);
            destination[4] = (int)(((uint)source[4] & Mask26) >> (26 - 2)) | ((source[5] & Mask26) << 2) | (source[6] << 28);
            destination[5] = (int)(((uint)source[6] & Mask26) >> (26 - 22)) | (source[7] << 22);
            destination[6] = (int)(((uint)source[7] & Mask26) >> (26 - 16)) | (source[8] << 16);
            destination[7] = (int)(((uint)source[8] & Mask26) >> (26 - 10)) | (source[9] << 10);
            destination[8] = (int)(((uint)source[9] & Mask26) >> (26 - 4)) | ((source[10] & Mask26) << 4) | (source[11] << 30);
            destination[9] = (int)(((uint)source[11] & Mask26) >> (26 - 24)) | (source[12] << 24);
            destination[10] = (int)(((uint)source[12] & Mask26) >> (26 - 18)) | (source[13] << 18);
            destination[11] = (int)(((uint)source[13] & Mask26) >> (26 - 12)) | (source[14] << 12);
            destination[12] = (int)(((uint)source[14] & Mask26) >> (26 - 6)) | (source[15] << 6);
            destination[13] = (source[16] & Mask26) | (source[17] << 26);
            destination[14] = (int)(((uint)source[17] & Mask26) >> (26 - 20)) | (source[18] << 20);
            destination[15] = (int)(((uint)source[18] & Mask26) >> (26 - 14)) | (source[19] << 14);
            destination[16] = (int)(((uint)source[19] & Mask26) >> (26 - 8)) | (source[20] << 8);
            destination[17] = (int)(((uint)source[20] & Mask26) >> (26 - 2)) | ((source[21] & Mask26) << 2) | (source[22] << 28);
            destination[18] = (int)(((uint)source[22] & Mask26) >> (26 - 22)) | (source[23] << 22);
            destination[19] = (int)(((uint)source[23] & Mask26) >> (26 - 16)) | (source[24] << 16);
            destination[20] = (int)(((uint)source[24] & Mask26) >> (26 - 10)) | (source[25] << 10);
            destination[21] = (int)(((uint)source[25] & Mask26) >> (26 - 4)) | ((source[26] & Mask26) << 4) | (source[27] << 30);
            destination[22] = (int)(((uint)source[27] & Mask26) >> (26 - 24)) | (source[28] << 24);
            destination[23] = (int)(((uint)source[28] & Mask26) >> (26 - 18)) | (source[29] << 18);
            destination[24] = (int)(((uint)source[29] & Mask26) >> (26 - 12)) | (source[30] << 12);
            destination[25] = (int)(((uint)source[30] & Mask26) >> (26 - 6)) | (source[31] << 6);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack27(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask27) | (source[1] << 27);
            destination[1] = (int)(((uint)source[1] & Mask27) >> (27 - 22)) | (source[2] << 22);
            destination[2] = (int)(((uint)source[2] & Mask27) >> (27 - 17)) | (source[3] << 17);
            destination[3] = (int)(((uint)source[3] & Mask27) >> (27 - 12)) | (source[4] << 12);
            destination[4] = (int)(((uint)source[4] & Mask27) >> (27 - 7)) | (source[5] << 7);
            destination[5] = (int)(((uint)source[5] & Mask27) >> (27 - 2)) | ((source[6] & Mask27) << 2) | (source[7] << 29);
            destination[6] = (int)(((uint)source[7] & Mask27) >> (27 - 24)) | (source[8] << 24);
            destination[7] = (int)(((uint)source[8] & Mask27) >> (27 - 19)) | (source[9] << 19);
            destination[8] = (int)(((uint)source[9] & Mask27) >> (27 - 14)) | (source[10] << 14);
            destination[9] = (int)(((uint)source[10] & Mask27) >> (27 - 9)) | (source[11] << 9);
            destination[10] = (int)(((uint)source[11] & Mask27) >> (27 - 4)) | ((source[12] & Mask27) << 4) | (source[13] << 31);
            destination[11] = (int)(((uint)source[13] & Mask27) >> (27 - 26)) | (source[14] << 26);
            destination[12] = (int)(((uint)source[14] & Mask27) >> (27 - 21)) | (source[15] << 21);
            destination[13] = (int)(((uint)source[15] & Mask27) >> (27 - 16)) | (source[16] << 16);
            destination[14] = (int)(((uint)source[16] & Mask27) >> (27 - 11)) | (source[17] << 11);
            destination[15] = (int)(((uint)source[17] & Mask27) >> (27 - 6)) | (source[18] << 6);
            destination[16] = (int)(((uint)source[18] & Mask27) >> (27 - 1)) | ((source[19] & Mask27) << 1) | (source[20] << 28);
            destination[17] = (int)(((uint)source[20] & Mask27) >> (27 - 23)) | (source[21] << 23);
            destination[18] = (int)(((uint)source[21] & Mask27) >> (27 - 18)) | (source[22] << 18);
            destination[19] = (int)(((uint)source[22] & Mask27) >> (27 - 13)) | (source[23] << 13);
            destination[20] = (int)(((uint)source[23] & Mask27) >> (27 - 8)) | (source[24] << 8);
            destination[21] = (int)(((uint)source[24] & Mask27) >> (27 - 3)) | ((source[25] & Mask27) << 3) | (source[26] << 30);
            destination[22] = (int)(((uint)source[26] & Mask27) >> (27 - 25)) | (source[27] << 25);
            destination[23] = (int)(((uint)source[27] & Mask27) >> (27 - 20)) | (source[28] << 20);
            destination[24] = (int)(((uint)source[28] & Mask27) >> (27 - 15)) | (source[29] << 15);
            destination[25] = (int)(((uint)source[29] & Mask27) >> (27 - 10)) | (source[30] << 10);
            destination[26] = (int)(((uint)source[30] & Mask27) >> (27 - 5)) | (source[31] << 5);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack28(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask28) | (source[1] << 28);
            destination[1] = (int)(((uint)source[1] & Mask28) >> (28 - 24)) | (source[2] << 24);
            destination[2] = (int)(((uint)source[2] & Mask28) >> (28 - 20)) | (source[3] << 20);
            destination[3] = (int)(((uint)source[3] & Mask28) >> (28 - 16)) | (source[4] << 16);
            destination[4] = (int)(((uint)source[4] & Mask28) >> (28 - 12)) | (source[5] << 12);
            destination[5] = (int)(((uint)source[5] & Mask28) >> (28 - 8)) | (source[6] << 8);
            destination[6] = (int)(((uint)source[6] & Mask28) >> (28 - 4)) | (source[7] << 4);
            destination[7] = (source[8] & Mask28) | (source[9] << 28);
            destination[8] = (int)(((uint)source[9] & Mask28) >> (28 - 24)) | (source[10] << 24);
            destination[9] = (int)(((uint)source[10] & Mask28) >> (28 - 20)) | (source[11] << 20);
            destination[10] = (int)(((uint)source[11] & Mask28) >> (28 - 16)) | (source[12] << 16);
            destination[11] = (int)(((uint)source[12] & Mask28) >> (28 - 12)) | (source[13] << 12);
            destination[12] = (int)(((uint)source[13] & Mask28) >> (28 - 8)) | (source[14] << 8);
            destination[13] = (int)(((uint)source[14] & Mask28) >> (28 - 4)) | (source[15] << 4);
            destination[14] = (source[16] & Mask28) | (source[17] << 28);
            destination[15] = (int)(((uint)source[17] & Mask28) >> (28 - 24)) | (source[18] << 24);
            destination[16] = (int)(((uint)source[18] & Mask28) >> (28 - 20)) | (source[19] << 20);
            destination[17] = (int)(((uint)source[19] & Mask28) >> (28 - 16)) | (source[20] << 16);
            destination[18] = (int)(((uint)source[20] & Mask28) >> (28 - 12)) | (source[21] << 12);
            destination[19] = (int)(((uint)source[21] & Mask28) >> (28 - 8)) | (source[22] << 8);
            destination[20] = (int)(((uint)source[22] & Mask28) >> (28 - 4)) | (source[23] << 4);
            destination[21] = (source[24] & Mask28) | (source[25] << 28);
            destination[22] = (int)(((uint)source[25] & Mask28) >> (28 - 24)) | (source[26] << 24);
            destination[23] = (int)(((uint)source[26] & Mask28) >> (28 - 20)) | (source[27] << 20);
            destination[24] = (int)(((uint)source[27] & Mask28) >> (28 - 16)) | (source[28] << 16);
            destination[25] = (int)(((uint)source[28] & Mask28) >> (28 - 12)) | (source[29] << 12);
            destination[26] = (int)(((uint)source[29] & Mask28) >> (28 - 8)) | (source[30] << 8);
            destination[27] = (int)(((uint)source[30] & Mask28) >> (28 - 4)) | (source[31] << 4);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack29(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask29) | (source[1] << 29);
            destination[1] = (int)(((uint)source[1] & Mask29) >> (29 - 26)) | (source[2] << 26);
            destination[2] = (int)(((uint)source[2] & Mask29) >> (29 - 23)) | (source[3] << 23);
            destination[3] = (int)(((uint)source[3] & Mask29) >> (29 - 20)) | (source[4] << 20);
            destination[4] = (int)(((uint)source[4] & Mask29) >> (29 - 17)) | (source[5] << 17);
            destination[5] = (int)(((uint)source[5] & Mask29) >> (29 - 14)) | (source[6] << 14);
            destination[6] = (int)(((uint)source[6] & Mask29) >> (29 - 11)) | (source[7] << 11);
            destination[7] = (int)(((uint)source[7] & Mask29) >> (29 - 8)) | (source[8] << 8);
            destination[8] = (int)(((uint)source[8] & Mask29) >> (29 - 5)) | (source[9] << 5);
            destination[9] = (int)(((uint)source[9] & Mask29) >> (29 - 2)) | ((source[10] & Mask29) << 2) | (source[11] << 31);
            destination[10] = (int)(((uint)source[11] & Mask29) >> (29 - 28)) | (source[12] << 28);
            destination[11] = (int)(((uint)source[12] & Mask29) >> (29 - 25)) | (source[13] << 25);
            destination[12] = (int)(((uint)source[13] & Mask29) >> (29 - 22)) | (source[14] << 22);
            destination[13] = (int)(((uint)source[14] & Mask29) >> (29 - 19)) | (source[15] << 19);
            destination[14] = (int)(((uint)source[15] & Mask29) >> (29 - 16)) | (source[16] << 16);
            destination[15] = (int)(((uint)source[16] & Mask29) >> (29 - 13)) | (source[17] << 13);
            destination[16] = (int)(((uint)source[17] & Mask29) >> (29 - 10)) | (source[18] << 10);
            destination[17] = (int)(((uint)source[18] & Mask29) >> (29 - 7)) | (source[19] << 7);
            destination[18] = (int)(((uint)source[19] & Mask29) >> (29 - 4)) | (source[20] << 4);
            destination[19] = (int)(((uint)source[20] & Mask29) >> (29 - 1)) | ((source[21] & Mask29) << 1) | (source[22] << 30);
            destination[20] = (int)(((uint)source[22] & Mask29) >> (29 - 27)) | (source[23] << 27);
            destination[21] = (int)(((uint)source[23] & Mask29) >> (29 - 24)) | (source[24] << 24);
            destination[22] = (int)(((uint)source[24] & Mask29) >> (29 - 21)) | (source[25] << 21);
            destination[23] = (int)(((uint)source[25] & Mask29) >> (29 - 18)) | (source[26] << 18);
            destination[24] = (int)(((uint)source[26] & Mask29) >> (29 - 15)) | (source[27] << 15);
            destination[25] = (int)(((uint)source[27] & Mask29) >> (29 - 12)) | (source[28] << 12);
            destination[26] = (int)(((uint)source[28] & Mask29) >> (29 - 9)) | (source[29] << 9);
            destination[27] = (int)(((uint)source[29] & Mask29) >> (29 - 6)) | (source[30] << 6);
            destination[28] = (int)(((uint)source[30] & Mask29) >> (29 - 3)) | (source[31] << 3);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack30(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask30) | (source[1] << 30);
            destination[1] = (int)(((uint)source[1] & Mask30) >> (30 - 28)) | (source[2] << 28);
            destination[2] = (int)(((uint)source[2] & Mask30) >> (30 - 26)) | (source[3] << 26);
            destination[3] = (int)(((uint)source[3] & Mask30) >> (30 - 24)) | (source[4] << 24);
            destination[4] = (int)(((uint)source[4] & Mask30) >> (30 - 22)) | (source[5] << 22);
            destination[5] = (int)(((uint)source[5] & Mask30) >> (30 - 20)) | (source[6] << 20);
            destination[6] = (int)(((uint)source[6] & Mask30) >> (30 - 18)) | (source[7] << 18);
            destination[7] = (int)(((uint)source[7] & Mask30) >> (30 - 16)) | (source[8] << 16);
            destination[8] = (int)(((uint)source[8] & Mask30) >> (30 - 14)) | (source[9] << 14);
            destination[9] = (int)(((uint)source[9] & Mask30) >> (30 - 12)) | (source[10] << 12);
            destination[10] = (int)(((uint)source[10] & Mask30) >> (30 - 10)) | (source[11] << 10);
            destination[11] = (int)(((uint)source[11] & Mask30) >> (30 - 8)) | (source[12] << 8);
            destination[12] = (int)(((uint)source[12] & Mask30) >> (30 - 6)) | (source[13] << 6);
            destination[13] = (int)(((uint)source[13] & Mask30) >> (30 - 4)) | (source[14] << 4);
            destination[14] = (int)(((uint)source[14] & Mask30) >> (30 - 2)) | (source[15] << 2);
            destination[15] = (source[16] & Mask30) | (source[17] << 30);
            destination[16] = (int)(((uint)source[17] & Mask30) >> (30 - 28)) | (source[18] << 28);
            destination[17] = (int)(((uint)source[18] & Mask30) >> (30 - 26)) | (source[19] << 26);
            destination[18] = (int)(((uint)source[19] & Mask30) >> (30 - 24)) | (source[20] << 24);
            destination[19] = (int)(((uint)source[20] & Mask30) >> (30 - 22)) | (source[21] << 22);
            destination[20] = (int)(((uint)source[21] & Mask30) >> (30 - 20)) | (source[22] << 20);
            destination[21] = (int)(((uint)source[22] & Mask30) >> (30 - 18)) | (source[23] << 18);
            destination[22] = (int)(((uint)source[23] & Mask30) >> (30 - 16)) | (source[24] << 16);
            destination[23] = (int)(((uint)source[24] & Mask30) >> (30 - 14)) | (source[25] << 14);
            destination[24] = (int)(((uint)source[25] & Mask30) >> (30 - 12)) | (source[26] << 12);
            destination[25] = (int)(((uint)source[26] & Mask30) >> (30 - 10)) | (source[27] << 10);
            destination[26] = (int)(((uint)source[27] & Mask30) >> (30 - 8)) | (source[28] << 8);
            destination[27] = (int)(((uint)source[28] & Mask30) >> (30 - 6)) | (source[29] << 6);
            destination[28] = (int)(((uint)source[29] & Mask30) >> (30 - 4)) | (source[30] << 4);
            destination[29] = (int)(((uint)source[30] & Mask30) >> (30 - 2)) | (source[31] << 2);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack31(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (source[0] & Mask31) | (source[1] << 31);
            destination[1] = (int)(((uint)source[1] & Mask31) >> (31 - 30)) | (source[2] << 30);
            destination[2] = (int)(((uint)source[2] & Mask31) >> (31 - 29)) | (source[3] << 29);
            destination[3] = (int)(((uint)source[3] & Mask31) >> (31 - 28)) | (source[4] << 28);
            destination[4] = (int)(((uint)source[4] & Mask31) >> (31 - 27)) | (source[5] << 27);
            destination[5] = (int)(((uint)source[5] & Mask31) >> (31 - 26)) | (source[6] << 26);
            destination[6] = (int)(((uint)source[6] & Mask31) >> (31 - 25)) | (source[7] << 25);
            destination[7] = (int)(((uint)source[7] & Mask31) >> (31 - 24)) | (source[8] << 24);
            destination[8] = (int)(((uint)source[8] & Mask31) >> (31 - 23)) | (source[9] << 23);
            destination[9] = (int)(((uint)source[9] & Mask31) >> (31 - 22)) | (source[10] << 22);
            destination[10] = (int)(((uint)source[10] & Mask31) >> (31 - 21)) | (source[11] << 21);
            destination[11] = (int)(((uint)source[11] & Mask31) >> (31 - 20)) | (source[12] << 20);
            destination[12] = (int)(((uint)source[12] & Mask31) >> (31 - 19)) | (source[13] << 19);
            destination[13] = (int)(((uint)source[13] & Mask31) >> (31 - 18)) | (source[14] << 18);
            destination[14] = (int)(((uint)source[14] & Mask31) >> (31 - 17)) | (source[15] << 17);
            destination[15] = (int)(((uint)source[15] & Mask31) >> (31 - 16)) | (source[16] << 16);
            destination[16] = (int)(((uint)source[16] & Mask31) >> (31 - 15)) | (source[17] << 15);
            destination[17] = (int)(((uint)source[17] & Mask31) >> (31 - 14)) | (source[18] << 14);
            destination[18] = (int)(((uint)source[18] & Mask31) >> (31 - 13)) | (source[19] << 13);
            destination[19] = (int)(((uint)source[19] & Mask31) >> (31 - 12)) | (source[20] << 12);
            destination[20] = (int)(((uint)source[20] & Mask31) >> (31 - 11)) | (source[21] << 11);
            destination[21] = (int)(((uint)source[21] & Mask31) >> (31 - 10)) | (source[22] << 10);
            destination[22] = (int)(((uint)source[22] & Mask31) >> (31 - 9)) | (source[23] << 9);
            destination[23] = (int)(((uint)source[23] & Mask31) >> (31 - 8)) | (source[24] << 8);
            destination[24] = (int)(((uint)source[24] & Mask31) >> (31 - 7)) | (source[25] << 7);
            destination[25] = (int)(((uint)source[25] & Mask31) >> (31 - 6)) | (source[26] << 6);
            destination[26] = (int)(((uint)source[26] & Mask31) >> (31 - 5)) | (source[27] << 5);
            destination[27] = (int)(((uint)source[27] & Mask31) >> (31 - 4)) | (source[28] << 4);
            destination[28] = (int)(((uint)source[28] & Mask31) >> (31 - 3)) | (source[29] << 3);
            destination[29] = (int)(((uint)source[29] & Mask31) >> (31 - 2)) | (source[30] << 2);
            destination[30] = (int)(((uint)source[30] & Mask31) >> (31 - 1)) | (source[31] << 1);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Pack32(ReadOnlySpan<int> source, Span<int> destination)
        {
            source[0..32].CopyTo(destination);
        }
    }

    /// <summary>
    /// Pack 32 integers.
    /// </summary>
    /// <param name="source">source array.</param>
    /// <param name="destination">output array.</param>
    /// <param name="bit">number of bits to use per integer.</param>
    public static void PackWithoutMask(ReadOnlySpan<int> source, Span<int> destination, int bit)
    {
        switch (bit)
        {
            case 0:
                break;
            case 1:
                PackWithoutMask1(source, destination);
                break;
            case 2:
                PackWithoutMask2(source, destination);
                break;
            case 3:
                PackWithoutMask3(source, destination);
                break;
            case 4:
                PackWithoutMask4(source, destination);
                break;
            case 5:
                PackWithoutMask5(source, destination);
                break;
            case 6:
                PackWithoutMask6(source, destination);
                break;
            case 7:
                PackWithoutMask7(source, destination);
                break;
            case 8:
                PackWithoutMask8(source, destination);
                break;
            case 9:
                PackWithoutMask9(source, destination);
                break;
            case 10:
                PackWithoutMask10(source, destination);
                break;
            case 11:
                PackWithoutMask11(source, destination);
                break;
            case 12:
                PackWithoutMask12(source, destination);
                break;
            case 13:
                PackWithoutMask13(source, destination);
                break;
            case 14:
                PackWithoutMask14(source, destination);
                break;
            case 15:
                PackWithoutMask15(source, destination);
                break;
            case 16:
                PackWithoutMask16(source, destination);
                break;
            case 17:
                PackWithoutMask17(source, destination);
                break;
            case 18:
                PackWithoutMask18(source, destination);
                break;
            case 19:
                PackWithoutMask19(source, destination);
                break;
            case 20:
                PackWithoutMask20(source, destination);
                break;
            case 21:
                PackWithoutMask21(source, destination);
                break;
            case 22:
                PackWithoutMask22(source, destination);
                break;
            case 23:
                PackWithoutMask23(source, destination);
                break;
            case 24:
                PackWithoutMask24(source, destination);
                break;
            case 25:
                PackWithoutMask25(source, destination);
                break;
            case 26:
                PackWithoutMask26(source, destination);
                break;
            case 27:
                PackWithoutMask27(source, destination);
                break;
            case 28:
                PackWithoutMask28(source, destination);
                break;
            case 29:
                PackWithoutMask29(source, destination);
                break;
            case 30:
                PackWithoutMask30(source, destination);
                break;
            case Mask5:
                PackWithoutMask31(source, destination);
                break;
            case 32:
                PackWithoutMask32(source, destination);
                break;
            default:
                throw new ArgumentException("Unsupported bit width.", paramName: nameof(bit));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask1(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 1)
                | (source[2] << 2) | (source[3] << 3)
                | (source[4] << 4) | (source[5] << 5)
                | (source[6] << 6) | (source[7] << 7)
                | (source[8] << 8) | (source[9] << 9)
                | (source[10] << 10) | (source[11] << 11)
                | (source[12] << 12) | (source[13] << 13)
                | (source[14] << 14) | (source[15] << 15)
                | (source[16] << 16) | (source[17] << 17)
                | (source[18] << 18) | (source[19] << 19)
                | (source[20] << 20) | (source[21] << 21)
                | (source[22] << 22) | (source[23] << 23)
                | (source[24] << 24) | (source[25] << 25)
                | (source[26] << 26) | (source[27] << 27)
                | (source[28] << 28) | (source[29] << 29)
                | (source[30] << 30) | (source[31] << 31);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask2(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 2)
                | (source[2] << 4) | (source[3] << 6)
                | (source[4] << 8) | (source[5] << 10)
                | (source[6] << 12) | (source[7] << 14)
                | (source[8] << 16) | (source[9] << 18)
                | (source[10] << 20) | (source[11] << 22)
                | (source[12] << 24) | (source[13] << 26)
                | (source[14] << 28) | (source[15] << 30);
            destination[1] = source[16] | (source[17] << 2)
                | (source[18] << 4) | (source[19] << 6)
                | (source[20] << 8) | (source[21] << 10)
                | (source[22] << 12) | (source[23] << 14)
                | (source[24] << 16) | (source[25] << 18)
                | (source[26] << 20) | (source[27] << 22)
                | (source[28] << 24) | (source[29] << 26)
                | (source[30] << 28) | (source[31] << 30);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask3(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 3)
                | (source[2] << 6) | (source[3] << 9)
                | (source[4] << 12) | (source[5] << 15)
                | (source[6] << 18) | (source[7] << 21)
                | (source[8] << 24) | (source[9] << 27)
                | (source[10] << 30);
            destination[1] = (int)((uint)source[10] >> (3 - 1))
                | (source[11] << 1) | (source[12] << 4)
                | (source[13] << 7) | (source[14] << 10)
                | (source[15] << 13) | (source[16] << 16)
                | (source[17] << 19) | (source[18] << 22)
                | (source[19] << 25) | (source[20] << 28)
                | (source[21] << 31);
            destination[2] = (int)((uint)source[21] >> (3 - 2))
                | (source[22] << 2) | (source[23] << 5)
                | (source[24] << 8) | (source[25] << 11)
                | (source[26] << 14) | (source[27] << 17)
                | (source[28] << 20) | (source[29] << 23)
                | (source[30] << 26) | (source[31] << 29);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask4(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 4)
                | (source[2] << 8) | (source[3] << 12)
                | (source[4] << 16) | (source[5] << 20)
                | (source[6] << 24) | (source[7] << 28);
            destination[1] = source[8] | (source[9] << 4)
                | (source[10] << 8) | (source[11] << 12)
                | (source[12] << 16) | (source[13] << 20)
                | (source[14] << 24) | (source[15] << 28);
            destination[2] = source[16] | (source[17] << 4)
                | (source[18] << 8) | (source[19] << 12)
                | (source[20] << 16) | (source[21] << 20)
                | (source[22] << 24) | (source[23] << 28);
            destination[3] = source[24] | (source[25] << 4)
                | (source[26] << 8) | (source[27] << 12)
                | (source[28] << 16) | (source[29] << 20)
                | (source[30] << 24) | (source[31] << 28);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask5(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 5)
                | (source[2] << 10) | (source[3] << 15)
                | (source[4] << 20) | (source[5] << 25)
                | (source[6] << 30);
            destination[1] = (int)((uint)source[6] >> (5 - 3))
                | (source[7] << 3) | (source[8] << 8)
                | (source[9] << 13) | (source[10] << 18)
                | (source[11] << 23) | (source[12] << 28);
            destination[2] = (int)((uint)source[12] >> (5 - 1))
                | (source[13] << 1) | (source[14] << 6)
                | (source[15] << 11) | (source[16] << 16)
                | (source[17] << 21) | (source[18] << 26)
                | (source[19] << 31);
            destination[3] = (int)((uint)source[19] >> (5 - 4))
                | (source[20] << 4) | (source[21] << 9)
                | (source[22] << 14) | (source[23] << 19)
                | (source[24] << 24) | (source[25] << 29);
            destination[4] = (int)((uint)source[25] >> (5 - 2))
                | (source[26] << 2) | (source[27] << 7)
                | (source[28] << 12) | (source[29] << 17)
                | (source[30] << 22) | (source[31] << 27);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask6(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 6)
                | (source[2] << 12) | (source[3] << 18)
                | (source[4] << 24) | (source[5] << 30);
            destination[1] = (int)((uint)source[5] >> (6 - 4))
                | (source[6] << 4) | (source[7] << 10)
                | (source[8] << 16) | (source[9] << 22)
                | (source[10] << 28);
            destination[2] = (int)((uint)source[10] >> (6 - 2))
                | (source[11] << 2) | (source[12] << 8)
                | (source[13] << 14) | (source[14] << 20)
                | (source[15] << 26);
            destination[3] = source[16] | (source[17] << 6)
                | (source[18] << 12) | (source[19] << 18)
                | (source[20] << 24) | (source[21] << 30);
            destination[4] = (int)((uint)source[21] >> (6 - 4))
                | (source[22] << 4) | (source[23] << 10)
                | (source[24] << 16) | (source[25] << 22)
                | (source[26] << 28);
            destination[5] = (int)((uint)source[26] >> (6 - 2))
                | (source[27] << 2) | (source[28] << 8)
                | (source[29] << 14) | (source[30] << 20)
                | (source[31] << 26);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask7(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 7)
                | (source[2] << 14) | (source[3] << 21)
                | (source[4] << 28);
            destination[1] = (int)((uint)source[4] >> (7 - 3))
                | (source[5] << 3) | (source[6] << 10)
                | (source[7] << 17) | (source[8] << 24)
                | (source[9] << 31);
            destination[2] = (int)((uint)source[9] >> (7 - 6))
                | (source[10] << 6) | (source[11] << 13)
                | (source[12] << 20) | (source[13] << 27);
            destination[3] = (int)((uint)source[13] >> (7 - 2))
                | (source[14] << 2) | (source[15] << 9)
                | (source[16] << 16) | (source[17] << 23)
                | (source[18] << 30);
            destination[4] = (int)((uint)source[18] >> (7 - 5))
                | (source[19] << 5) | (source[20] << 12)
                | (source[21] << 19) | (source[22] << 26);
            destination[5] = (int)((uint)source[22] >> (7 - 1))
                | (source[23] << 1) | (source[24] << 8)
                | (source[25] << 15) | (source[26] << 22)
                | (source[27] << 29);
            destination[6] = (int)((uint)source[27] >> (7 - 4))
                | (source[28] << 4) | (source[29] << 11)
                | (source[30] << 18) | (source[31] << 25);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask8(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 8) | (source[2] << 16) | (source[3] << 24);
            destination[1] = source[4] | (source[5] << 8) | (source[6] << 16) | (source[7] << 24);
            destination[2] = source[8] | (source[9] << 8) | (source[10] << 16) | (source[11] << 24);
            destination[3] = source[12] | (source[13] << 8) | (source[14] << 16) | (source[15] << 24);
            destination[4] = source[16] | (source[17] << 8) | (source[18] << 16) | (source[19] << 24);
            destination[5] = source[20] | (source[21] << 8) | (source[22] << 16) | (source[23] << 24);
            destination[6] = source[24] | (source[25] << 8) | (source[26] << 16) | (source[27] << 24);
            destination[7] = source[28] | (source[29] << 8) | (source[30] << 16) | (source[31] << 24);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask9(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 9)
                | (source[2] << 18) | (source[3] << 27);
            destination[1] = (int)((uint)source[3] >> (9 - 4))
                | (source[4] << 4) | (source[5] << 13)
                | (source[6] << 22) | (source[7] << 31);
            destination[2] = (int)((uint)source[7] >> (9 - 8))
                | (source[8] << 8) | (source[9] << 17)
                | (source[10] << 26);
            destination[3] = (int)((uint)source[10] >> (9 - 3))
                | (source[11] << 3) | (source[12] << 12)
                | (source[13] << 21) | (source[14] << 30);
            destination[4] = (int)((uint)source[14] >> (9 - 7))
                | (source[15] << 7) | (source[16] << 16)
                | (source[17] << 25);
            destination[5] = (int)((uint)source[17] >> (9 - 2))
                | (source[18] << 2) | (source[19] << 11)
                | (source[20] << 20) | (source[21] << 29);
            destination[6] = (int)((uint)source[21] >> (9 - 6))
                | (source[22] << 6) | (source[23] << 15)
                | (source[24] << 24);
            destination[7] = (int)((uint)source[24] >> (9 - 1))
                | (source[25] << 1) | (source[26] << 10)
                | (source[27] << 19) | (source[28] << 28);
            destination[8] = (int)((uint)source[28] >> (9 - 5))
                | (source[29] << 5) | (source[30] << 14)
                | (source[31] << 23);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask10(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 10)
                | (source[2] << 20) | (source[3] << 30);
            destination[1] = (int)((uint)source[3] >> (10 - 8))
                | (source[4] << 8) | (source[5] << 18)
                | (source[6] << 28);
            destination[2] = (int)((uint)source[6] >> (10 - 6))
                | (source[7] << 6) | (source[8] << 16)
                | (source[9] << 26);
            destination[3] = (int)((uint)source[9] >> (10 - 4))
                | (source[10] << 4) | (source[11] << 14)
                | (source[12] << 24);
            destination[4] = (int)((uint)source[12] >> (10 - 2))
                | (source[13] << 2) | (source[14] << 12)
                | (source[15] << 22);
            destination[5] = source[16] | (source[17] << 10)
                | (source[18] << 20) | (source[19] << 30);
            destination[6] = (int)((uint)source[19] >> (10 - 8))
                | (source[20] << 8) | (source[21] << 18)
                | (source[22] << 28);
            destination[7] = (int)((uint)source[22] >> (10 - 6))
                | (source[23] << 6) | (source[24] << 16)
                | (source[25] << 26);
            destination[8] = (int)((uint)source[25] >> (10 - 4))
                | (source[26] << 4) | (source[27] << 14)
                | (source[28] << 24);
            destination[9] = (int)((uint)source[28] >> (10 - 2))
                | (source[29] << 2) | (source[30] << 12)
                | (source[31] << 22);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask11(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 11)
                | (source[2] << 22);
            destination[1] = (int)((uint)source[2] >> (11 - 1))
                | (source[3] << 1) | (source[4] << 12)
                | (source[5] << 23);
            destination[2] = (int)((uint)source[5] >> (11 - 2))
                | (source[6] << 2) | (source[7] << 13)
                | (source[8] << 24);
            destination[3] = (int)((uint)source[8] >> (11 - 3))
                | (source[9] << 3) | (source[10] << 14)
                | (source[11] << 25);
            destination[4] = (int)((uint)source[11] >> (11 - 4))
                | (source[12] << 4) | (source[13] << 15)
                | (source[14] << 26);
            destination[5] = (int)((uint)source[14] >> (11 - 5))
                | (source[15] << 5) | (source[16] << 16)
                | (source[17] << 27);
            destination[6] = (int)((uint)source[17] >> (11 - 6))
                | (source[18] << 6) | (source[19] << 17)
                | (source[20] << 28);
            destination[7] = (int)((uint)source[20] >> (11 - 7))
                | (source[21] << 7) | (source[22] << 18)
                | (source[23] << 29);
            destination[8] = (int)((uint)source[23] >> (11 - 8))
                | (source[24] << 8) | (source[25] << 19)
                | (source[26] << 30);
            destination[9] = (int)((uint)source[26] >> (11 - 9))
                | (source[27] << 9) | (source[28] << 20)
                | (source[29] << 31);
            destination[10] = (int)((uint)source[29] >> (11 - 10))
                | (source[30] << 10) | (source[31] << 21);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask12(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 12)
                | (source[2] << 24);
            destination[1] = (int)((uint)source[2] >> (12 - 4))
                | (source[3] << 4) | (source[4] << 16)
                | (source[5] << 28);
            destination[2] = (int)((uint)source[5] >> (12 - 8))
                | (source[6] << 8) | (source[7] << 20);
            destination[3] = source[8] | (source[9] << 12)
                | (source[10] << 24);
            destination[4] = (int)((uint)source[10] >> (12 - 4))
                | (source[11] << 4) | (source[12] << 16)
                | (source[13] << 28);
            destination[5] = (int)((uint)source[13] >> (12 - 8))
                | (source[14] << 8) | (source[15] << 20);
            destination[6] = source[16] | (source[17] << 12)
                | (source[18] << 24);
            destination[7] = (int)((uint)source[18] >> (12 - 4))
                | (source[19] << 4) | (source[20] << 16)
                | (source[21] << 28);
            destination[8] = (int)((uint)source[21] >> (12 - 8))
                | (source[22] << 8) | (source[23] << 20);
            destination[9] = source[24] | (source[25] << 12)
                | (source[26] << 24);
            destination[10] = (int)((uint)source[26] >> (12 - 4))
                | (source[27] << 4) | (source[28] << 16)
                | (source[29] << 28);
            destination[11] = (int)((uint)source[29] >> (12 - 8))
                | (source[30] << 8) | (source[31] << 20);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask13(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 13)
                | (source[2] << 26);
            destination[1] = (int)((uint)source[2] >> (13 - 7))
                | (source[3] << 7) | (source[4] << 20);
            destination[2] = (int)((uint)source[4] >> (13 - 1))
                | (source[5] << 1) | (source[6] << 14)
                | (source[7] << 27);
            destination[3] = (int)((uint)source[7] >> (13 - 8))
                | (source[8] << 8) | (source[9] << 21);
            destination[4] = (int)((uint)source[9] >> (13 - 2))
                | (source[10] << 2) | (source[11] << 15)
                | (source[12] << 28);
            destination[5] = (int)((uint)source[12] >> (13 - 9))
                | (source[13] << 9) | (source[14] << 22);
            destination[6] = (int)((uint)source[14] >> (13 - 3))
                | (source[15] << 3) | (source[16] << 16)
                | (source[17] << 29);
            destination[7] = (int)((uint)source[17] >> (13 - 10))
                | (source[18] << 10) | (source[19] << 23);
            destination[8] = (int)((uint)source[19] >> (13 - 4))
                | (source[20] << 4) | (source[21] << 17)
                | (source[22] << 30);
            destination[9] = (int)((uint)source[22] >> (13 - 11))
                | (source[23] << 11) | (source[24] << 24);
            destination[10] = (int)((uint)source[24] >> (13 - 5))
                | (source[25] << 5) | (source[26] << 18)
                | (source[27] << 31);
            destination[11] = (int)((uint)source[27] >> (13 - 12))
                | (source[28] << 12) | (source[29] << 25);
            destination[12] = (int)((uint)source[29] >> (13 - 6))
                | (source[30] << 6) | (source[31] << 19);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask14(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 14)
                | (source[2] << 28);
            destination[1] = (int)((uint)source[2] >> (14 - 10))
                | (source[3] << 10) | (source[4] << 24);
            destination[2] = (int)((uint)source[4] >> (14 - 6))
                | (source[5] << 6) | (source[6] << 20);
            destination[3] = (int)((uint)source[6] >> (14 - 2))
                | (source[7] << 2) | (source[8] << 16)
                | (source[9] << 30);
            destination[4] = (int)((uint)source[9] >> (14 - 12))
                | (source[10] << 12) | (source[11] << 26);
            destination[5] = (int)((uint)source[11] >> (14 - 8))
                | (source[12] << 8) | (source[13] << 22);
            destination[6] = (int)((uint)source[13] >> (14 - 4))
                | (source[14] << 4) | (source[15] << 18);
            destination[7] = source[16] | (source[17] << 14)
                | (source[18] << 28);
            destination[8] = (int)((uint)source[18] >> (14 - 10))
                | (source[19] << 10) | (source[20] << 24);
            destination[9] = (int)((uint)source[20] >> (14 - 6))
                | (source[21] << 6) | (source[22] << 20);
            destination[10] = (int)((uint)source[22] >> (14 - 2))
                | (source[23] << 2) | (source[24] << 16)
                | (source[25] << 30);
            destination[11] = (int)((uint)source[25] >> (14 - 12))
                | (source[26] << 12) | (source[27] << 26);
            destination[12] = (int)((uint)source[27] >> (14 - 8))
                | (source[28] << 8) | (source[29] << 22);
            destination[13] = (int)((uint)source[29] >> (14 - 4))
                | (source[30] << 4) | (source[31] << 18);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask15(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 15)
                | (source[2] << 30);
            destination[1] = (int)((uint)source[2] >> (15 - 13))
                | (source[3] << 13) | (source[4] << 28);
            destination[2] = (int)((uint)source[4] >> (15 - 11))
                | (source[5] << 11) | (source[6] << 26);
            destination[3] = (int)((uint)source[6] >> (15 - 9))
                | (source[7] << 9) | (source[8] << 24);
            destination[4] = (int)((uint)source[8] >> (15 - 7))
                | (source[9] << 7) | (source[10] << 22);
            destination[5] = (int)((uint)source[10] >> (15 - 5))
                | (source[11] << 5) | (source[12] << 20);
            destination[6] = (int)((uint)source[12] >> (15 - 3))
                | (source[13] << 3) | (source[14] << 18);
            destination[7] = (int)((uint)source[14] >> (15 - 1))
                | (source[15] << 1) | (source[16] << 16)
                | (source[17] << 31);
            destination[8] = (int)((uint)source[17] >> (15 - 14))
                | (source[18] << 14) | (source[19] << 29);
            destination[9] = (int)((uint)source[19] >> (15 - 12))
                | (source[20] << 12) | (source[21] << 27);
            destination[10] = (int)((uint)source[21] >> (15 - 10))
                | (source[22] << 10) | (source[23] << 25);
            destination[11] = (int)((uint)source[23] >> (15 - 8))
                | (source[24] << 8) | (source[25] << 23);
            destination[12] = (int)((uint)source[25] >> (15 - 6))
                | (source[26] << 6) | (source[27] << 21);
            destination[13] = (int)((uint)source[27] >> (15 - 4))
                | (source[28] << 4) | (source[29] << 19);
            destination[14] = (int)((uint)source[29] >> (15 - 2))
                | (source[30] << 2) | (source[31] << 17);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask16(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 16);
            destination[1] = source[2] | (source[3] << 16);
            destination[2] = source[4] | (source[5] << 16);
            destination[3] = source[6] | (source[7] << 16);
            destination[4] = source[8] | (source[9] << 16);
            destination[5] = source[10] | (source[11] << 16);
            destination[6] = source[12] | (source[13] << 16);
            destination[7] = source[14] | (source[15] << 16);
            destination[8] = source[16] | (source[17] << 16);
            destination[9] = source[18] | (source[19] << 16);
            destination[10] = source[20] | (source[21] << 16);
            destination[11] = source[22] | (source[23] << 16);
            destination[12] = source[24] | (source[25] << 16);
            destination[13] = source[26] | (source[27] << 16);
            destination[14] = source[28] | (source[29] << 16);
            destination[15] = source[30] | (source[31] << 16);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask17(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 17);
            destination[1] = (int)((uint)source[1] >> (17 - 2))
                | (source[2] << 2) | (source[3] << 19);
            destination[2] = (int)((uint)source[3] >> (17 - 4))
                | (source[4] << 4) | (source[5] << 21);
            destination[3] = (int)((uint)source[5] >> (17 - 6))
                | (source[6] << 6) | (source[7] << 23);
            destination[4] = (int)((uint)source[7] >> (17 - 8))
                | (source[8] << 8) | (source[9] << 25);
            destination[5] = (int)((uint)source[9] >> (17 - 10))
                | (source[10] << 10) | (source[11] << 27);
            destination[6] = (int)((uint)source[11] >> (17 - 12))
                | (source[12] << 12) | (source[13] << 29);
            destination[7] = (int)((uint)source[13] >> (17 - 14))
                | (source[14] << 14) | (source[15] << 31);
            destination[8] = (int)((uint)source[15] >> (17 - 16))
                | (source[16] << 16);
            destination[9] = (int)((uint)source[16] >> (17 - 1))
                | (source[17] << 1) | (source[18] << 18);
            destination[10] = (int)((uint)source[18] >> (17 - 3))
                | (source[19] << 3) | (source[20] << 20);
            destination[11] = (int)((uint)source[20] >> (17 - 5))
                | (source[21] << 5) | (source[22] << 22);
            destination[12] = (int)((uint)source[22] >> (17 - 7))
                | (source[23] << 7) | (source[24] << 24);
            destination[13] = (int)((uint)source[24] >> (17 - 9))
                | (source[25] << 9) | (source[26] << 26);
            destination[14] = (int)((uint)source[26] >> (17 - 11))
                | (source[27] << 11) | (source[28] << 28);
            destination[15] = (int)((uint)source[28] >> (17 - 13))
                | (source[29] << 13) | (source[30] << 30);
            destination[16] = (int)((uint)source[30] >> (17 - 15))
                | (source[31] << 15);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask18(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 18);
            destination[1] = (int)((uint)source[1] >> (18 - 4))
                | (source[2] << 4) | (source[3] << 22);
            destination[2] = (int)((uint)source[3] >> (18 - 8))
                | (source[4] << 8) | (source[5] << 26);
            destination[3] = (int)((uint)source[5] >> (18 - 12))
                | (source[6] << 12) | (source[7] << 30);
            destination[4] = (int)((uint)source[7] >> (18 - 16))
                | (source[8] << 16);
            destination[5] = (int)((uint)source[8] >> (18 - 2))
                | (source[9] << 2) | (source[10] << 20);
            destination[6] = (int)((uint)source[10] >> (18 - 6))
                | (source[11] << 6) | (source[12] << 24);
            destination[7] = (int)((uint)source[12] >> (18 - 10))
                | (source[13] << 10) | (source[14] << 28);
            destination[8] = (int)((uint)source[14] >> (18 - 14))
                | (source[15] << 14);
            destination[9] = source[16] | (source[17] << 18);
            destination[10] = (int)((uint)source[17] >> (18 - 4))
                | (source[18] << 4) | (source[19] << 22);
            destination[11] = (int)((uint)source[19] >> (18 - 8))
                | (source[20] << 8) | (source[21] << 26);
            destination[12] = (int)((uint)source[21] >> (18 - 12))
                | (source[22] << 12) | (source[23] << 30);
            destination[13] = (int)((uint)source[23] >> (18 - 16))
                | (source[24] << 16);
            destination[14] = (int)((uint)source[24] >> (18 - 2))
                | (source[25] << 2) | (source[26] << 20);
            destination[15] = (int)((uint)source[26] >> (18 - 6))
                | (source[27] << 6) | (source[28] << 24);
            destination[16] = (int)((uint)source[28] >> (18 - 10))
                | (source[29] << 10) | (source[30] << 28);
            destination[17] = (int)((uint)source[30] >> (18 - 14))
                | (source[31] << 14);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask19(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 19);
            destination[1] = (int)((uint)source[1] >> (19 - 6))
                | (source[2] << 6) | (source[3] << 25);
            destination[2] = (int)((uint)source[3] >> (19 - 12))
                | (source[4] << 12) | (source[5] << 31);
            destination[3] = (int)((uint)source[5] >> (19 - 18))
                | (source[6] << 18);
            destination[4] = (int)((uint)source[6] >> (19 - 5))
                | (source[7] << 5) | (source[8] << 24);
            destination[5] = (int)((uint)source[8] >> (19 - 11))
                | (source[9] << 11) | (source[10] << 30);
            destination[6] = (int)((uint)source[10] >> (19 - 17))
                | (source[11] << 17);
            destination[7] = (int)((uint)source[11] >> (19 - 4))
                | (source[12] << 4) | (source[13] << 23);
            destination[8] = (int)((uint)source[13] >> (19 - 10))
                | (source[14] << 10) | (source[15] << 29);
            destination[9] = (int)((uint)source[15] >> (19 - 16))
                | (source[16] << 16);
            destination[10] = (int)((uint)source[16] >> (19 - 3))
                | (source[17] << 3) | (source[18] << 22);
            destination[11] = (int)((uint)source[18] >> (19 - 9))
                | (source[19] << 9) | (source[20] << 28);
            destination[12] = (int)((uint)source[20] >> (19 - 15))
                | (source[21] << 15);
            destination[13] = (int)((uint)source[21] >> (19 - 2))
                | (source[22] << 2) | (source[23] << 21);
            destination[14] = (int)((uint)source[23] >> (19 - 8))
                | (source[24] << 8) | (source[25] << 27);
            destination[15] = (int)((uint)source[25] >> (19 - 14))
                | (source[26] << 14);
            destination[16] = (int)((uint)source[26] >> (19 - 1))
                | (source[27] << 1) | (source[28] << 20);
            destination[17] = (int)((uint)source[28] >> (19 - 7))
                | (source[29] << 7) | (source[30] << 26);
            destination[18] = (int)((uint)source[30] >> (19 - 13))
                | (source[31] << 13);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask20(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 20);
            destination[1] = (int)((uint)source[1] >> (20 - 8))
                | (source[2] << 8) | (source[3] << 28);
            destination[2] = (int)((uint)source[3] >> (20 - 16))
                | (source[4] << 16);
            destination[3] = (int)((uint)source[4] >> (20 - 4))
                | (source[5] << 4) | (source[6] << 24);
            destination[4] = (int)((uint)source[6] >> (20 - 12))
                | (source[7] << 12);
            destination[5] = source[8] | (source[9] << 20);
            destination[6] = (int)((uint)source[9] >> (20 - 8))
                | (source[10] << 8) | (source[11] << 28);
            destination[7] = (int)((uint)source[11] >> (20 - 16))
                | (source[12] << 16);
            destination[8] = (int)((uint)source[12] >> (20 - 4))
                | (source[13] << 4) | (source[14] << 24);
            destination[9] = (int)((uint)source[14] >> (20 - 12))
                | (source[15] << 12);
            destination[10] = source[16] | (source[17] << 20);
            destination[11] = (int)((uint)source[17] >> (20 - 8))
                | (source[18] << 8) | (source[19] << 28);
            destination[12] = (int)((uint)source[19] >> (20 - 16))
                | (source[20] << 16);
            destination[13] = (int)((uint)source[20] >> (20 - 4))
                | (source[21] << 4) | (source[22] << 24);
            destination[14] = (int)((uint)source[22] >> (20 - 12))
                | (source[23] << 12);
            destination[15] = source[24] | (source[25] << 20);
            destination[16] = (int)((uint)source[25] >> (20 - 8))
                | (source[26] << 8) | (source[27] << 28);
            destination[17] = (int)((uint)source[27] >> (20 - 16))
                | (source[28] << 16);
            destination[18] = (int)((uint)source[28] >> (20 - 4))
                | (source[29] << 4) | (source[30] << 24);
            destination[19] = (int)((uint)source[30] >> (20 - 12))
                | (source[31] << 12);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask21(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 21);
            destination[1] = (int)((uint)source[1] >> (21 - 10))
                | (source[2] << 10) | (source[3] << 31);
            destination[2] = (int)((uint)source[3] >> (21 - 20))
                | (source[4] << 20);
            destination[3] = (int)((uint)source[4] >> (21 - 9))
                | (source[5] << 9) | (source[6] << 30);
            destination[4] = (int)((uint)source[6] >> (21 - 19))
                | (source[7] << 19);
            destination[5] = (int)((uint)source[7] >> (21 - 8))
                | (source[8] << 8) | (source[9] << 29);
            destination[6] = (int)((uint)source[9] >> (21 - 18))
                | (source[10] << 18);
            destination[7] = (int)((uint)source[10] >> (21 - 7))
                | (source[11] << 7) | (source[12] << 28);
            destination[8] = (int)((uint)source[12] >> (21 - 17))
                | (source[13] << 17);
            destination[9] = (int)((uint)source[13] >> (21 - 6))
                | (source[14] << 6) | (source[15] << 27);
            destination[10] = (int)((uint)source[15] >> (21 - 16))
                | (source[16] << 16);
            destination[11] = (int)((uint)source[16] >> (21 - 5))
                | (source[17] << 5) | (source[18] << 26);
            destination[12] = (int)((uint)source[18] >> (21 - 15))
                | (source[19] << 15);
            destination[13] = (int)((uint)source[19] >> (21 - 4))
                | (source[20] << 4) | (source[21] << 25);
            destination[14] = (int)((uint)source[21] >> (21 - 14))
                | (source[22] << 14);
            destination[15] = (int)((uint)source[22] >> (21 - 3))
                | (source[23] << 3) | (source[24] << 24);
            destination[16] = (int)((uint)source[24] >> (21 - 13))
                | (source[25] << 13);
            destination[17] = (int)((uint)source[25] >> (21 - 2))
                | (source[26] << 2) | (source[27] << 23);
            destination[18] = (int)((uint)source[27] >> (21 - 12))
                | (source[28] << 12);
            destination[19] = (int)((uint)source[28] >> (21 - 1))
                | (source[29] << 1) | (source[30] << 22);
            destination[20] = (int)((uint)source[30] >> (21 - 11))
                | (source[31] << 11);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask22(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 22);
            destination[1] = (int)((uint)source[1] >> (22 - 12))
                | (source[2] << 12);
            destination[2] = (int)((uint)source[2] >> (22 - 2))
                | (source[3] << 2) | (source[4] << 24);
            destination[3] = (int)((uint)source[4] >> (22 - 14))
                | (source[5] << 14);
            destination[4] = (int)((uint)source[5] >> (22 - 4))
                | (source[6] << 4) | (source[7] << 26);
            destination[5] = (int)((uint)source[7] >> (22 - 16))
                | (source[8] << 16);
            destination[6] = (int)((uint)source[8] >> (22 - 6))
                | (source[9] << 6) | (source[10] << 28);
            destination[7] = (int)((uint)source[10] >> (22 - 18))
                | (source[11] << 18);
            destination[8] = (int)((uint)source[11] >> (22 - 8))
                | (source[12] << 8) | (source[13] << 30);
            destination[9] = (int)((uint)source[13] >> (22 - 20))
                | (source[14] << 20);
            destination[10] = (int)((uint)source[14] >> (22 - 10))
                | (source[15] << 10);
            destination[11] = source[16] | (source[17] << 22);
            destination[12] = (int)((uint)source[17] >> (22 - 12))
                | (source[18] << 12);
            destination[13] = (int)((uint)source[18] >> (22 - 2))
                | (source[19] << 2) | (source[20] << 24);
            destination[14] = (int)((uint)source[20] >> (22 - 14))
                | (source[21] << 14);
            destination[15] = (int)((uint)source[21] >> (22 - 4))
                | (source[22] << 4) | (source[23] << 26);
            destination[16] = (int)((uint)source[23] >> (22 - 16))
                | (source[24] << 16);
            destination[17] = (int)((uint)source[24] >> (22 - 6))
                | (source[25] << 6) | (source[26] << 28);
            destination[18] = (int)((uint)source[26] >> (22 - 18))
                | (source[27] << 18);
            destination[19] = (int)((uint)source[27] >> (22 - 8))
                | (source[28] << 8) | (source[29] << 30);
            destination[20] = (int)((uint)source[29] >> (22 - 20))
                | (source[30] << 20);
            destination[21] = (int)((uint)source[30] >> (22 - 10))
                | (source[31] << 10);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask23(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 23);
            destination[1] = (int)((uint)source[1] >> (23 - 14))
                | (source[2] << 14);
            destination[2] = (int)((uint)source[2] >> (23 - 5))
                | (source[3] << 5) | (source[4] << 28);
            destination[3] = (int)((uint)source[4] >> (23 - 19))
                | (source[5] << 19);
            destination[4] = (int)((uint)source[5] >> (23 - 10))
                | (source[6] << 10);
            destination[5] = (int)((uint)source[6] >> (23 - 1))
                | (source[7] << 1) | (source[8] << 24);
            destination[6] = (int)((uint)source[8] >> (23 - 15))
                | (source[9] << 15);
            destination[7] = (int)((uint)source[9] >> (23 - 6))
                | (source[10] << 6) | (source[11] << 29);
            destination[8] = (int)((uint)source[11] >> (23 - 20))
                | (source[12] << 20);
            destination[9] = (int)((uint)source[12] >> (23 - 11))
                | (source[13] << 11);
            destination[10] = (int)((uint)source[13] >> (23 - 2))
                | (source[14] << 2) | (source[15] << 25);
            destination[11] = (int)((uint)source[15] >> (23 - 16))
                | (source[16] << 16);
            destination[12] = (int)((uint)source[16] >> (23 - 7))
                | (source[17] << 7) | (source[18] << 30);
            destination[13] = (int)((uint)source[18] >> (23 - 21))
                | (source[19] << 21);
            destination[14] = (int)((uint)source[19] >> (23 - 12))
                | (source[20] << 12);
            destination[15] = (int)((uint)source[20] >> (23 - 3))
                | (source[21] << 3) | (source[22] << 26);
            destination[16] = (int)((uint)source[22] >> (23 - 17))
                | (source[23] << 17);
            destination[17] = (int)((uint)source[23] >> (23 - 8))
                | (source[24] << 8) | (source[25] << 31);
            destination[18] = (int)((uint)source[25] >> (23 - 22))
                | (source[26] << 22);
            destination[19] = (int)((uint)source[26] >> (23 - 13))
                | (source[27] << 13);
            destination[20] = (int)((uint)source[27] >> (23 - 4))
                | (source[28] << 4) | (source[29] << 27);
            destination[21] = (int)((uint)source[29] >> (23 - 18))
                | (source[30] << 18);
            destination[22] = (int)((uint)source[30] >> (23 - 9))
                | (source[31] << 9);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask24(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 24);
            destination[1] = (int)((uint)source[1] >> (24 - 16))
                | (source[2] << 16);
            destination[2] = (int)((uint)source[2] >> (24 - 8))
                | (source[3] << 8);
            destination[3] = source[4] | (source[5] << 24);
            destination[4] = (int)((uint)source[5] >> (24 - 16))
                | (source[6] << 16);
            destination[5] = (int)((uint)source[6] >> (24 - 8))
                | (source[7] << 8);
            destination[6] = source[8] | (source[9] << 24);
            destination[7] = (int)((uint)source[9] >> (24 - 16))
                | (source[10] << 16);
            destination[8] = (int)((uint)source[10] >> (24 - 8))
                | (source[11] << 8);
            destination[9] = source[12] | (source[13] << 24);
            destination[10] = (int)((uint)source[13] >> (24 - 16))
                | (source[14] << 16);
            destination[11] = (int)((uint)source[14] >> (24 - 8))
                | (source[15] << 8);
            destination[12] = source[16] | (source[17] << 24);
            destination[13] = (int)((uint)source[17] >> (24 - 16))
                | (source[18] << 16);
            destination[14] = (int)((uint)source[18] >> (24 - 8))
                | (source[19] << 8);
            destination[15] = source[20] | (source[21] << 24);
            destination[16] = (int)((uint)source[21] >> (24 - 16))
                | (source[22] << 16);
            destination[17] = (int)((uint)source[22] >> (24 - 8))
                | (source[23] << 8);
            destination[18] = source[24] | (source[25] << 24);
            destination[19] = (int)((uint)source[25] >> (24 - 16))
                | (source[26] << 16);
            destination[20] = (int)((uint)source[26] >> (24 - 8))
                | (source[27] << 8);
            destination[21] = source[28] | (source[29] << 24);
            destination[22] = (int)((uint)source[29] >> (24 - 16))
                | (source[30] << 16);
            destination[23] = (int)((uint)source[30] >> (24 - 8))
                | (source[31] << 8);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask25(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 25);
            destination[1] = (int)((uint)source[1] >> (25 - 18))
                | (source[2] << 18);
            destination[2] = (int)((uint)source[2] >> (25 - 11))
                | (source[3] << 11);
            destination[3] = (int)((uint)source[3] >> (25 - 4))
                | (source[4] << 4) | (source[5] << 29);
            destination[4] = (int)((uint)source[5] >> (25 - 22))
                | (source[6] << 22);
            destination[5] = (int)((uint)source[6] >> (25 - 15))
                | (source[7] << 15);
            destination[6] = (int)((uint)source[7] >> (25 - 8))
                | (source[8] << 8);
            destination[7] = (int)((uint)source[8] >> (25 - 1))
                | (source[9] << 1) | (source[10] << 26);
            destination[8] = (int)((uint)source[10] >> (25 - 19))
                | (source[11] << 19);
            destination[9] = (int)((uint)source[11] >> (25 - 12))
                | (source[12] << 12);
            destination[10] = (int)((uint)source[12] >> (25 - 5))
                | (source[13] << 5) | (source[14] << 30);
            destination[11] = (int)((uint)source[14] >> (25 - 23))
                | (source[15] << 23);
            destination[12] = (int)((uint)source[15] >> (25 - 16))
                | (source[16] << 16);
            destination[13] = (int)((uint)source[16] >> (25 - 9))
                | (source[17] << 9);
            destination[14] = (int)((uint)source[17] >> (25 - 2))
                | (source[18] << 2) | (source[19] << 27);
            destination[15] = (int)((uint)source[19] >> (25 - 20))
                | (source[20] << 20);
            destination[16] = (int)((uint)source[20] >> (25 - 13))
                | (source[21] << 13);
            destination[17] = (int)((uint)source[21] >> (25 - 6))
                | (source[22] << 6) | (source[23] << 31);
            destination[18] = (int)((uint)source[23] >> (25 - 24))
                | (source[24] << 24);
            destination[19] = (int)((uint)source[24] >> (25 - 17))
                | (source[25] << 17);
            destination[20] = (int)((uint)source[25] >> (25 - 10))
                | (source[26] << 10);
            destination[21] = (int)((uint)source[26] >> (25 - 3))
                | (source[27] << 3) | (source[28] << 28);
            destination[22] = (int)((uint)source[28] >> (25 - 21))
                | (source[29] << 21);
            destination[23] = (int)((uint)source[29] >> (25 - 14))
                | (source[30] << 14);
            destination[24] = (int)((uint)source[30] >> (25 - 7))
                | (source[31] << 7);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask26(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 26);
            destination[1] = (int)((uint)source[1] >> (26 - 20))
                | (source[2] << 20);
            destination[2] = (int)((uint)source[2] >> (26 - 14))
                | (source[3] << 14);
            destination[3] = (int)((uint)source[3] >> (26 - 8))
                | (source[4] << 8);
            destination[4] = (int)((uint)source[4] >> (26 - 2))
                | (source[5] << 2) | (source[6] << 28);
            destination[5] = (int)((uint)source[6] >> (26 - 22))
                | (source[7] << 22);
            destination[6] = (int)((uint)source[7] >> (26 - 16))
                | (source[8] << 16);
            destination[7] = (int)((uint)source[8] >> (26 - 10))
                | (source[9] << 10);
            destination[8] = (int)((uint)source[9] >> (26 - 4))
                | (source[10] << 4) | (source[11] << 30);
            destination[9] = (int)((uint)source[11] >> (26 - 24))
                | (source[12] << 24);
            destination[10] = (int)((uint)source[12] >> (26 - 18))
                | (source[13] << 18);
            destination[11] = (int)((uint)source[13] >> (26 - 12))
                | (source[14] << 12);
            destination[12] = (int)((uint)source[14] >> (26 - 6))
                | (source[15] << 6);
            destination[13] = source[16] | (source[17] << 26);
            destination[14] = (int)((uint)source[17] >> (26 - 20))
                | (source[18] << 20);
            destination[15] = (int)((uint)source[18] >> (26 - 14))
                | (source[19] << 14);
            destination[16] = (int)((uint)source[19] >> (26 - 8))
                | (source[20] << 8);
            destination[17] = (int)((uint)source[20] >> (26 - 2))
                | (source[21] << 2) | (source[22] << 28);
            destination[18] = (int)((uint)source[22] >> (26 - 22))
                | (source[23] << 22);
            destination[19] = (int)((uint)source[23] >> (26 - 16))
                | (source[24] << 16);
            destination[20] = (int)((uint)source[24] >> (26 - 10))
                | (source[25] << 10);
            destination[21] = (int)((uint)source[25] >> (26 - 4))
                | (source[26] << 4) | (source[27] << 30);
            destination[22] = (int)((uint)source[27] >> (26 - 24))
                | (source[28] << 24);
            destination[23] = (int)((uint)source[28] >> (26 - 18))
                | (source[29] << 18);
            destination[24] = (int)((uint)source[29] >> (26 - 12))
                | (source[30] << 12);
            destination[25] = (int)((uint)source[30] >> (26 - 6))
                | (source[31] << 6);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask27(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 27);
            destination[1] = (int)((uint)source[1] >> (27 - 22))
                | (source[2] << 22);
            destination[2] = (int)((uint)source[2] >> (27 - 17))
                | (source[3] << 17);
            destination[3] = (int)((uint)source[3] >> (27 - 12))
                | (source[4] << 12);
            destination[4] = (int)((uint)source[4] >> (27 - 7))
                | (source[5] << 7);
            destination[5] = (int)((uint)source[5] >> (27 - 2))
                | (source[6] << 2) | (source[7] << 29);
            destination[6] = (int)((uint)source[7] >> (27 - 24))
                | (source[8] << 24);
            destination[7] = (int)((uint)source[8] >> (27 - 19))
                | (source[9] << 19);
            destination[8] = (int)((uint)source[9] >> (27 - 14))
                | (source[10] << 14);
            destination[9] = (int)((uint)source[10] >> (27 - 9))
                | (source[11] << 9);
            destination[10] = (int)((uint)source[11] >> (27 - 4))
                | (source[12] << 4) | (source[13] << 31);
            destination[11] = (int)((uint)source[13] >> (27 - 26))
                | (source[14] << 26);
            destination[12] = (int)((uint)source[14] >> (27 - 21))
                | (source[15] << 21);
            destination[13] = (int)((uint)source[15] >> (27 - 16))
                | (source[16] << 16);
            destination[14] = (int)((uint)source[16] >> (27 - 11))
                | (source[17] << 11);
            destination[15] = (int)((uint)source[17] >> (27 - 6))
                | (source[18] << 6);
            destination[16] = (int)((uint)source[18] >> (27 - 1))
                | (source[19] << 1) | (source[20] << 28);
            destination[17] = (int)((uint)source[20] >> (27 - 23))
                | (source[21] << 23);
            destination[18] = (int)((uint)source[21] >> (27 - 18))
                | (source[22] << 18);
            destination[19] = (int)((uint)source[22] >> (27 - 13))
                | (source[23] << 13);
            destination[20] = (int)((uint)source[23] >> (27 - 8))
                | (source[24] << 8);
            destination[21] = (int)((uint)source[24] >> (27 - 3))
                | (source[25] << 3) | (source[26] << 30);
            destination[22] = (int)((uint)source[26] >> (27 - 25))
                | (source[27] << 25);
            destination[23] = (int)((uint)source[27] >> (27 - 20))
                | (source[28] << 20);
            destination[24] = (int)((uint)source[28] >> (27 - 15))
                | (source[29] << 15);
            destination[25] = (int)((uint)source[29] >> (27 - 10))
                | (source[30] << 10);
            destination[26] = (int)((uint)source[30] >> (27 - 5))
                | (source[31] << 5);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask28(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 28);
            destination[1] = (int)((uint)source[1] >> (28 - 24))
                | (source[2] << 24);
            destination[2] = (int)((uint)source[2] >> (28 - 20))
                | (source[3] << 20);
            destination[3] = (int)((uint)source[3] >> (28 - 16))
                | (source[4] << 16);
            destination[4] = (int)((uint)source[4] >> (28 - 12))
                | (source[5] << 12);
            destination[5] = (int)((uint)source[5] >> (28 - 8))
                | (source[6] << 8);
            destination[6] = (int)((uint)source[6] >> (28 - 4))
                | (source[7] << 4);
            destination[7] = source[8] | (source[9] << 28);
            destination[8] = (int)((uint)source[9] >> (28 - 24))
                | (source[10] << 24);
            destination[9] = (int)((uint)source[10] >> (28 - 20))
                | (source[11] << 20);
            destination[10] = (int)((uint)source[11] >> (28 - 16))
                | (source[12] << 16);
            destination[11] = (int)((uint)source[12] >> (28 - 12))
                | (source[13] << 12);
            destination[12] = (int)((uint)source[13] >> (28 - 8))
                | (source[14] << 8);
            destination[13] = (int)((uint)source[14] >> (28 - 4))
                | (source[15] << 4);
            destination[14] = source[16] | (source[17] << 28);
            destination[15] = (int)((uint)source[17] >> (28 - 24))
                | (source[18] << 24);
            destination[16] = (int)((uint)source[18] >> (28 - 20))
                | (source[19] << 20);
            destination[17] = (int)((uint)source[19] >> (28 - 16))
                | (source[20] << 16);
            destination[18] = (int)((uint)source[20] >> (28 - 12))
                | (source[21] << 12);
            destination[19] = (int)((uint)source[21] >> (28 - 8))
                | (source[22] << 8);
            destination[20] = (int)((uint)source[22] >> (28 - 4))
                | (source[23] << 4);
            destination[21] = source[24] | (source[25] << 28);
            destination[22] = (int)((uint)source[25] >> (28 - 24))
                | (source[26] << 24);
            destination[23] = (int)((uint)source[26] >> (28 - 20))
                | (source[27] << 20);
            destination[24] = (int)((uint)source[27] >> (28 - 16))
                | (source[28] << 16);
            destination[25] = (int)((uint)source[28] >> (28 - 12))
                | (source[29] << 12);
            destination[26] = (int)((uint)source[29] >> (28 - 8))
                | (source[30] << 8);
            destination[27] = (int)((uint)source[30] >> (28 - 4))
                | (source[31] << 4);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask29(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 29);
            destination[1] = (int)((uint)source[1] >> (29 - 26))
                | (source[2] << 26);
            destination[2] = (int)((uint)source[2] >> (29 - 23))
                | (source[3] << 23);
            destination[3] = (int)((uint)source[3] >> (29 - 20))
                | (source[4] << 20);
            destination[4] = (int)((uint)source[4] >> (29 - 17))
                | (source[5] << 17);
            destination[5] = (int)((uint)source[5] >> (29 - 14))
                | (source[6] << 14);
            destination[6] = (int)((uint)source[6] >> (29 - 11))
                | (source[7] << 11);
            destination[7] = (int)((uint)source[7] >> (29 - 8))
                | (source[8] << 8);
            destination[8] = (int)((uint)source[8] >> (29 - 5))
                | (source[9] << 5);
            destination[9] = (int)((uint)source[9] >> (29 - 2))
                | (source[10] << 2) | (source[11] << 31);
            destination[10] = (int)((uint)source[11] >> (29 - 28))
                | (source[12] << 28);
            destination[11] = (int)((uint)source[12] >> (29 - 25))
                | (source[13] << 25);
            destination[12] = (int)((uint)source[13] >> (29 - 22))
                | (source[14] << 22);
            destination[13] = (int)((uint)source[14] >> (29 - 19))
                | (source[15] << 19);
            destination[14] = (int)((uint)source[15] >> (29 - 16))
                | (source[16] << 16);
            destination[15] = (int)((uint)source[16] >> (29 - 13))
                | (source[17] << 13);
            destination[16] = (int)((uint)source[17] >> (29 - 10))
                | (source[18] << 10);
            destination[17] = (int)((uint)source[18] >> (29 - 7))
                | (source[19] << 7);
            destination[18] = (int)((uint)source[19] >> (29 - 4))
                | (source[20] << 4);
            destination[19] = (int)((uint)source[20] >> (29 - 1))
                | (source[21] << 1) | (source[22] << 30);
            destination[20] = (int)((uint)source[22] >> (29 - 27))
                | (source[23] << 27);
            destination[21] = (int)((uint)source[23] >> (29 - 24))
                | (source[24] << 24);
            destination[22] = (int)((uint)source[24] >> (29 - 21))
                | (source[25] << 21);
            destination[23] = (int)((uint)source[25] >> (29 - 18))
                | (source[26] << 18);
            destination[24] = (int)((uint)source[26] >> (29 - 15))
                | (source[27] << 15);
            destination[25] = (int)((uint)source[27] >> (29 - 12))
                | (source[28] << 12);
            destination[26] = (int)((uint)source[28] >> (29 - 9))
                | (source[29] << 9);
            destination[27] = (int)((uint)source[29] >> (29 - 6))
                | (source[30] << 6);
            destination[28] = (int)((uint)source[30] >> (29 - 3))
                | (source[31] << 3);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask30(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 30);
            destination[1] = (int)((uint)source[1] >> (30 - 28))
                | (source[2] << 28);
            destination[2] = (int)((uint)source[2] >> (30 - 26))
                | (source[3] << 26);
            destination[3] = (int)((uint)source[3] >> (30 - 24))
                | (source[4] << 24);
            destination[4] = (int)((uint)source[4] >> (30 - 22))
                | (source[5] << 22);
            destination[5] = (int)((uint)source[5] >> (30 - 20))
                | (source[6] << 20);
            destination[6] = (int)((uint)source[6] >> (30 - 18))
                | (source[7] << 18);
            destination[7] = (int)((uint)source[7] >> (30 - 16))
                | (source[8] << 16);
            destination[8] = (int)((uint)source[8] >> (30 - 14))
                | (source[9] << 14);
            destination[9] = (int)((uint)source[9] >> (30 - 12))
                | (source[10] << 12);
            destination[10] = (int)((uint)source[10] >> (30 - 10))
                | (source[11] << 10);
            destination[11] = (int)((uint)source[11] >> (30 - 8))
                | (source[12] << 8);
            destination[12] = (int)((uint)source[12] >> (30 - 6))
                | (source[13] << 6);
            destination[13] = (int)((uint)source[13] >> (30 - 4))
                | (source[14] << 4);
            destination[14] = (int)((uint)source[14] >> (30 - 2))
                | (source[15] << 2);
            destination[15] = source[16] | (source[17] << 30);
            destination[16] = (int)((uint)source[17] >> (30 - 28))
                | (source[18] << 28);
            destination[17] = (int)((uint)source[18] >> (30 - 26))
                | (source[19] << 26);
            destination[18] = (int)((uint)source[19] >> (30 - 24))
                | (source[20] << 24);
            destination[19] = (int)((uint)source[20] >> (30 - 22))
                | (source[21] << 22);
            destination[20] = (int)((uint)source[21] >> (30 - 20))
                | (source[22] << 20);
            destination[21] = (int)((uint)source[22] >> (30 - 18))
                | (source[23] << 18);
            destination[22] = (int)((uint)source[23] >> (30 - 16))
                | (source[24] << 16);
            destination[23] = (int)((uint)source[24] >> (30 - 14))
                | (source[25] << 14);
            destination[24] = (int)((uint)source[25] >> (30 - 12))
                | (source[26] << 12);
            destination[25] = (int)((uint)source[26] >> (30 - 10))
                | (source[27] << 10);
            destination[26] = (int)((uint)source[27] >> (30 - 8))
                | (source[28] << 8);
            destination[27] = (int)((uint)source[28] >> (30 - 6))
                | (source[29] << 6);
            destination[28] = (int)((uint)source[29] >> (30 - 4))
                | (source[30] << 4);
            destination[29] = (int)((uint)source[30] >> (30 - 2))
                | (source[31] << 2);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask31(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = source[0] | (source[1] << 31);
            destination[1] = (int)((uint)source[1] >> (31 - 30)) | (source[2] << 30);
            destination[2] = (int)((uint)source[2] >> (31 - 29)) | (source[3] << 29);
            destination[3] = (int)((uint)source[3] >> (31 - 28)) | (source[4] << 28);
            destination[4] = (int)((uint)source[4] >> (31 - 27)) | (source[5] << 27);
            destination[5] = (int)((uint)source[5] >> (31 - 26)) | (source[6] << 26);
            destination[6] = (int)((uint)source[6] >> (31 - 25)) | (source[7] << 25);
            destination[7] = (int)((uint)source[7] >> (31 - 24)) | (source[8] << 24);
            destination[8] = (int)((uint)source[8] >> (31 - 23)) | (source[9] << 23);
            destination[9] = (int)((uint)source[9] >> (31 - 22)) | (source[10] << 22);
            destination[10] = (int)((uint)source[10] >> (31 - 21)) | (source[11] << 21);
            destination[11] = (int)((uint)source[11] >> (31 - 20)) | (source[12] << 20);
            destination[12] = (int)((uint)source[12] >> (31 - 19)) | (source[13] << 19);
            destination[13] = (int)((uint)source[13] >> (31 - 18)) | (source[14] << 18);
            destination[14] = (int)((uint)source[14] >> (31 - 17)) | (source[15] << 17);
            destination[15] = (int)((uint)source[15] >> (31 - 16)) | (source[16] << 16);
            destination[16] = (int)((uint)source[16] >> (31 - 15)) | (source[17] << 15);
            destination[17] = (int)((uint)source[17] >> (31 - 14)) | (source[18] << 14);
            destination[18] = (int)((uint)source[18] >> (31 - 13)) | (source[19] << 13);
            destination[19] = (int)((uint)source[19] >> (31 - 12)) | (source[20] << 12);
            destination[20] = (int)((uint)source[20] >> (31 - 11)) | (source[21] << 11);
            destination[21] = (int)((uint)source[21] >> (31 - 10)) | (source[22] << 10);
            destination[22] = (int)((uint)source[22] >> (31 - 9)) | (source[23] << 9);
            destination[23] = (int)((uint)source[23] >> (31 - 8)) | (source[24] << 8);
            destination[24] = (int)((uint)source[24] >> (31 - 7)) | (source[25] << 7);
            destination[25] = (int)((uint)source[25] >> (31 - 6)) | (source[26] << 6);
            destination[26] = (int)((uint)source[26] >> (31 - 5)) | (source[27] << 5);
            destination[27] = (int)((uint)source[27] >> (31 - 4)) | (source[28] << 4);
            destination[28] = (int)((uint)source[28] >> (31 - 3)) | (source[29] << 3);
            destination[29] = (int)((uint)source[29] >> (31 - 2)) | (source[30] << 2);
            destination[30] = (int)((uint)source[30] >> (31 - 1)) | (source[31] << 1);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void PackWithoutMask32(ReadOnlySpan<int> source, Span<int> destination)
        {
            source[0..32].CopyTo(destination);
        }
    }

    /// <summary>
    /// Unpack 32 integers.
    /// </summary>
    /// <param name="source">source array.</param>
    /// <param name="destination">output array.</param>
    /// <param name="bit">number of bits to use per integer.</param>
    public static void Unpack(ReadOnlySpan<int> source, Span<int> destination, int bit)
    {
        switch (bit)
        {
            case 0:
                Unpack0(destination);
                break;
            case 1:
                Unpack1(source, destination);
                break;
            case 2:
                Unpack2(source, destination);
                break;
            case 3:
                Unpack3(source, destination);
                break;
            case 4:
                Unpack4(source, destination);
                break;
            case 5:
                Unpack5(source, destination);
                break;
            case 6:
                Unpack6(source, destination);
                break;
            case 7:
                Unpack7(source, destination);
                break;
            case 8:
                Unpack8(source, destination);
                break;
            case 9:
                Unpack9(source, destination);
                break;
            case 10:
                Unpack10(source, destination);
                break;
            case 11:
                Unpack11(source, destination);
                break;
            case 12:
                Unpack12(source, destination);
                break;
            case 13:
                Unpack13(source, destination);
                break;
            case 14:
                Unpack14(source, destination);
                break;
            case 15:
                Unpack15(source, destination);
                break;
            case 16:
                Unpack16(source, destination);
                break;
            case 17:
                Unpack17(source, destination);
                break;
            case 18:
                Unpack18(source, destination);
                break;
            case 19:
                Unpack19(source, destination);
                break;
            case 20:
                Unpack20(source, destination);
                break;
            case 21:
                Unpack21(source, destination);
                break;
            case 22:
                Unpack22(source, destination);
                break;
            case 23:
                Unpack23(source, destination);
                break;
            case 24:
                Unpack24(source, destination);
                break;
            case 25:
                Unpack25(source, destination);
                break;
            case 26:
                Unpack26(source, destination);
                break;
            case 27:
                Unpack27(source, destination);
                break;
            case 28:
                Unpack28(source, destination);
                break;
            case 29:
                Unpack29(source, destination);
                break;
            case 30:
                Unpack30(source, destination);
                break;
            case 31:
                Unpack31(source, destination);
                break;
            case 32:
                Unpack32(source, destination);
                break;
            default:
                throw new ArgumentException("Unsupported bit width.", paramName: nameof(bit));
        }

        static void Unpack0(Span<int> destination)
        {
            destination[..32].Clear();
        }

        static void Unpack1(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask1);
            destination[1] = (int)(((uint)source[0] >> 1) & Mask1);
            destination[2] = (int)(((uint)source[0] >> 2) & Mask1);
            destination[3] = (int)(((uint)source[0] >> 3) & Mask1);
            destination[4] = (int)(((uint)source[0] >> 4) & Mask1);
            destination[5] = (int)(((uint)source[0] >> 5) & Mask1);
            destination[6] = (int)(((uint)source[0] >> 6) & Mask1);
            destination[7] = (int)(((uint)source[0] >> 7) & Mask1);
            destination[8] = (int)(((uint)source[0] >> 8) & Mask1);
            destination[9] = (int)(((uint)source[0] >> 9) & Mask1);
            destination[10] = (int)(((uint)source[0] >> 10) & Mask1);
            destination[11] = (int)(((uint)source[0] >> 11) & Mask1);
            destination[12] = (int)(((uint)source[0] >> 12) & Mask1);
            destination[13] = (int)(((uint)source[0] >> 13) & Mask1);
            destination[14] = (int)(((uint)source[0] >> 14) & Mask1);
            destination[15] = (int)(((uint)source[0] >> 15) & Mask1);
            destination[16] = (int)(((uint)source[0] >> 16) & Mask1);
            destination[17] = (int)(((uint)source[0] >> 17) & Mask1);
            destination[18] = (int)(((uint)source[0] >> 18) & Mask1);
            destination[19] = (int)(((uint)source[0] >> 19) & Mask1);
            destination[20] = (int)(((uint)source[0] >> 20) & Mask1);
            destination[21] = (int)(((uint)source[0] >> 21) & Mask1);
            destination[22] = (int)(((uint)source[0] >> 22) & Mask1);
            destination[23] = (int)(((uint)source[0] >> 23) & Mask1);
            destination[24] = (int)(((uint)source[0] >> 24) & Mask1);
            destination[25] = (int)(((uint)source[0] >> 25) & Mask1);
            destination[26] = (int)(((uint)source[0] >> 26) & Mask1);
            destination[27] = (int)(((uint)source[0] >> 27) & Mask1);
            destination[28] = (int)(((uint)source[0] >> 28) & Mask1);
            destination[29] = (int)(((uint)source[0] >> 29) & Mask1);
            destination[30] = (int)(((uint)source[0] >> 30) & Mask1);
            destination[31] = (int)((uint)source[0] >> 31);
        }

        static void Unpack2(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask2);
            destination[1] = (int)(((uint)source[0] >> 2) & Mask2);
            destination[2] = (int)(((uint)source[0] >> 4) & Mask2);
            destination[3] = (int)(((uint)source[0] >> 6) & Mask2);
            destination[4] = (int)(((uint)source[0] >> 8) & Mask2);
            destination[5] = (int)(((uint)source[0] >> 10) & Mask2);
            destination[6] = (int)(((uint)source[0] >> 12) & Mask2);
            destination[7] = (int)(((uint)source[0] >> 14) & Mask2);
            destination[8] = (int)(((uint)source[0] >> 16) & Mask2);
            destination[9] = (int)(((uint)source[0] >> 18) & Mask2);
            destination[10] = (int)(((uint)source[0] >> 20) & Mask2);
            destination[11] = (int)(((uint)source[0] >> 22) & Mask2);
            destination[12] = (int)(((uint)source[0] >> 24) & Mask2);
            destination[13] = (int)(((uint)source[0] >> 26) & Mask2);
            destination[14] = (int)(((uint)source[0] >> 28) & Mask2);
            destination[15] = (int)((uint)source[0] >> 30);
            destination[16] = (int)(((uint)source[1] >> 0) & Mask2);
            destination[17] = (int)(((uint)source[1] >> 2) & Mask2);
            destination[18] = (int)(((uint)source[1] >> 4) & Mask2);
            destination[19] = (int)(((uint)source[1] >> 6) & Mask2);
            destination[20] = (int)(((uint)source[1] >> 8) & Mask2);
            destination[21] = (int)(((uint)source[1] >> 10) & Mask2);
            destination[22] = (int)(((uint)source[1] >> 12) & Mask2);
            destination[23] = (int)(((uint)source[1] >> 14) & Mask2);
            destination[24] = (int)(((uint)source[1] >> 16) & Mask2);
            destination[25] = (int)(((uint)source[1] >> 18) & Mask2);
            destination[26] = (int)(((uint)source[1] >> 20) & Mask2);
            destination[27] = (int)(((uint)source[1] >> 22) & Mask2);
            destination[28] = (int)(((uint)source[1] >> 24) & Mask2);
            destination[29] = (int)(((uint)source[1] >> 26) & Mask2);
            destination[30] = (int)(((uint)source[1] >> 28) & Mask2);
            destination[31] = (int)((uint)source[1] >> 30);
        }

        static void Unpack3(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask3);
            destination[1] = (int)(((uint)source[0] >> 3) & Mask3);
            destination[2] = (int)(((uint)source[0] >> 6) & Mask3);
            destination[3] = (int)(((uint)source[0] >> 9) & Mask3);
            destination[4] = (int)(((uint)source[0] >> 12) & Mask3);
            destination[5] = (int)(((uint)source[0] >> 15) & Mask3);
            destination[6] = (int)(((uint)source[0] >> 18) & Mask3);
            destination[7] = (int)(((uint)source[0] >> 21) & Mask3);
            destination[8] = (int)(((uint)source[0] >> 24) & Mask3);
            destination[9] = (int)(((uint)source[0] >> 27) & Mask3);
            destination[10] = (int)((uint)source[0] >> 30) | ((source[1] & Mask1) << (3 - 1));
            destination[11] = (int)(((uint)source[1] >> 1) & Mask3);
            destination[12] = (int)(((uint)source[1] >> 4) & Mask3);
            destination[13] = (int)(((uint)source[1] >> 7) & Mask3);
            destination[14] = (int)(((uint)source[1] >> 10) & Mask3);
            destination[15] = (int)(((uint)source[1] >> 13) & Mask3);
            destination[16] = (int)(((uint)source[1] >> 16) & Mask3);
            destination[17] = (int)(((uint)source[1] >> 19) & Mask3);
            destination[18] = (int)(((uint)source[1] >> 22) & Mask3);
            destination[19] = (int)(((uint)source[1] >> 25) & Mask3);
            destination[20] = (int)(((uint)source[1] >> 28) & Mask3);
            destination[21] = (int)((uint)source[1] >> 31) | ((source[2] & Mask2) << (3 - 2));
            destination[22] = (int)(((uint)source[2] >> 2) & Mask3);
            destination[23] = (int)(((uint)source[2] >> 5) & Mask3);
            destination[24] = (int)(((uint)source[2] >> 8) & Mask3);
            destination[25] = (int)(((uint)source[2] >> 11) & Mask3);
            destination[26] = (int)(((uint)source[2] >> 14) & Mask3);
            destination[27] = (int)(((uint)source[2] >> 17) & Mask3);
            destination[28] = (int)(((uint)source[2] >> 20) & Mask3);
            destination[29] = (int)(((uint)source[2] >> 23) & Mask3);
            destination[30] = (int)(((uint)source[2] >> 26) & Mask3);
            destination[31] = (int)((uint)source[2] >> 29);
        }

        static void Unpack4(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask4);
            destination[1] = (int)(((uint)source[0] >> 4) & Mask4);
            destination[2] = (int)(((uint)source[0] >> 8) & Mask4);
            destination[3] = (int)(((uint)source[0] >> 12) & Mask4);
            destination[4] = (int)(((uint)source[0] >> 16) & Mask4);
            destination[5] = (int)(((uint)source[0] >> 20) & Mask4);
            destination[6] = (int)(((uint)source[0] >> 24) & Mask4);
            destination[7] = (int)((uint)source[0] >> 28);
            destination[8] = (int)(((uint)source[1] >> 0) & Mask4);
            destination[9] = (int)(((uint)source[1] >> 4) & Mask4);
            destination[10] = (int)(((uint)source[1] >> 8) & Mask4);
            destination[11] = (int)(((uint)source[1] >> 12) & Mask4);
            destination[12] = (int)(((uint)source[1] >> 16) & Mask4);
            destination[13] = (int)(((uint)source[1] >> 20) & Mask4);
            destination[14] = (int)(((uint)source[1] >> 24) & Mask4);
            destination[15] = (int)((uint)source[1] >> 28);
            destination[16] = (int)(((uint)source[2] >> 0) & Mask4);
            destination[17] = (int)(((uint)source[2] >> 4) & Mask4);
            destination[18] = (int)(((uint)source[2] >> 8) & Mask4);
            destination[19] = (int)(((uint)source[2] >> 12) & Mask4);
            destination[20] = (int)(((uint)source[2] >> 16) & Mask4);
            destination[21] = (int)(((uint)source[2] >> 20) & Mask4);
            destination[22] = (int)(((uint)source[2] >> 24) & Mask4);
            destination[23] = (int)((uint)source[2] >> 28);
            destination[24] = (int)(((uint)source[3] >> 0) & Mask4);
            destination[25] = (int)(((uint)source[3] >> 4) & Mask4);
            destination[26] = (int)(((uint)source[3] >> 8) & Mask4);
            destination[27] = (int)(((uint)source[3] >> 12) & Mask4);
            destination[28] = (int)(((uint)source[3] >> 16) & Mask4);
            destination[29] = (int)(((uint)source[3] >> 20) & Mask4);
            destination[30] = (int)(((uint)source[3] >> 24) & Mask4);
            destination[31] = (int)((uint)source[3] >> 28);
        }

        static void Unpack5(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask5);
            destination[1] = (int)(((uint)source[0] >> 5) & Mask5);
            destination[2] = (int)(((uint)source[0] >> 10) & Mask5);
            destination[3] = (int)(((uint)source[0] >> 15) & Mask5);
            destination[4] = (int)(((uint)source[0] >> 20) & Mask5);
            destination[5] = (int)(((uint)source[0] >> 25) & Mask5);
            destination[6] = (int)((uint)source[0] >> 30) | ((source[1] & Mask3) << (5 - 3));
            destination[7] = (int)(((uint)source[1] >> 3) & Mask5);
            destination[8] = (int)(((uint)source[1] >> 8) & Mask5);
            destination[9] = (int)(((uint)source[1] >> 13) & Mask5);
            destination[10] = (int)(((uint)source[1] >> 18) & Mask5);
            destination[11] = (int)(((uint)source[1] >> 23) & Mask5);
            destination[12] = (int)((uint)source[1] >> 28) | ((source[2] & Mask1) << (5 - 1));
            destination[13] = (int)(((uint)source[2] >> 1) & Mask5);
            destination[14] = (int)(((uint)source[2] >> 6) & Mask5);
            destination[15] = (int)(((uint)source[2] >> 11) & Mask5);
            destination[16] = (int)(((uint)source[2] >> 16) & Mask5);
            destination[17] = (int)(((uint)source[2] >> 21) & Mask5);
            destination[18] = (int)(((uint)source[2] >> 26) & Mask5);
            destination[19] = (int)((uint)source[2] >> 31) | ((source[3] & Mask4) << (5 - 4));
            destination[20] = (int)(((uint)source[3] >> 4) & Mask5);
            destination[21] = (int)(((uint)source[3] >> 9) & Mask5);
            destination[22] = (int)(((uint)source[3] >> 14) & Mask5);
            destination[23] = (int)(((uint)source[3] >> 19) & Mask5);
            destination[24] = (int)(((uint)source[3] >> 24) & Mask5);
            destination[25] = (int)((uint)source[3] >> 29) | ((source[4] & Mask2) << (5 - 2));
            destination[26] = (int)(((uint)source[4] >> 2) & Mask5);
            destination[27] = (int)(((uint)source[4] >> 7) & Mask5);
            destination[28] = (int)(((uint)source[4] >> 12) & Mask5);
            destination[29] = (int)(((uint)source[4] >> 17) & Mask5);
            destination[30] = (int)(((uint)source[4] >> 22) & Mask5);
            destination[31] = (int)((uint)source[4] >> 27);
        }

        static void Unpack6(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask6);
            destination[1] = (int)(((uint)source[0] >> 6) & Mask6);
            destination[2] = (int)(((uint)source[0] >> 12) & Mask6);
            destination[3] = (int)(((uint)source[0] >> 18) & Mask6);
            destination[4] = (int)(((uint)source[0] >> 24) & Mask6);
            destination[5] = (int)((uint)source[0] >> 30) | ((source[1] & Mask4) << (6 - 4));
            destination[6] = (int)(((uint)source[1] >> 4) & Mask6);
            destination[7] = (int)(((uint)source[1] >> 10) & Mask6);
            destination[8] = (int)(((uint)source[1] >> 16) & Mask6);
            destination[9] = (int)(((uint)source[1] >> 22) & Mask6);
            destination[10] = (int)((uint)source[1] >> 28) | ((source[2] & Mask2) << (6 - 2));
            destination[11] = (int)(((uint)source[2] >> 2) & Mask6);
            destination[12] = (int)(((uint)source[2] >> 8) & Mask6);
            destination[13] = (int)(((uint)source[2] >> 14) & Mask6);
            destination[14] = (int)(((uint)source[2] >> 20) & Mask6);
            destination[15] = (int)((uint)source[2] >> 26);
            destination[16] = (int)(((uint)source[3] >> 0) & Mask6);
            destination[17] = (int)(((uint)source[3] >> 6) & Mask6);
            destination[18] = (int)(((uint)source[3] >> 12) & Mask6);
            destination[19] = (int)(((uint)source[3] >> 18) & Mask6);
            destination[20] = (int)(((uint)source[3] >> 24) & Mask6);
            destination[21] = (int)((uint)source[3] >> 30) | ((source[4] & Mask4) << (6 - 4));
            destination[22] = (int)(((uint)source[4] >> 4) & Mask6);
            destination[23] = (int)(((uint)source[4] >> 10) & Mask6);
            destination[24] = (int)(((uint)source[4] >> 16) & Mask6);
            destination[25] = (int)(((uint)source[4] >> 22) & Mask6);
            destination[26] = (int)((uint)source[4] >> 28) | ((source[5] & Mask2) << (6 - 2));
            destination[27] = (int)(((uint)source[5] >> 2) & Mask6);
            destination[28] = (int)(((uint)source[5] >> 8) & Mask6);
            destination[29] = (int)(((uint)source[5] >> 14) & Mask6);
            destination[30] = (int)(((uint)source[5] >> 20) & Mask6);
            destination[31] = (int)((uint)source[5] >> 26);
        }

        static void Unpack7(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask7);
            destination[1] = (int)(((uint)source[0] >> 7) & Mask7);
            destination[2] = (int)(((uint)source[0] >> 14) & Mask7);
            destination[3] = (int)(((uint)source[0] >> 21) & Mask7);
            destination[4] = (int)((uint)source[0] >> 28) | ((source[1] & Mask3) << (7 - 3));
            destination[5] = (int)(((uint)source[1] >> 3) & Mask7);
            destination[6] = (int)(((uint)source[1] >> 10) & Mask7);
            destination[7] = (int)(((uint)source[1] >> 17) & Mask7);
            destination[8] = (int)(((uint)source[1] >> 24) & Mask7);
            destination[9] = (int)((uint)source[1] >> 31) | ((source[2] & Mask6) << (7 - 6));
            destination[10] = (int)(((uint)source[2] >> 6) & Mask7);
            destination[11] = (int)(((uint)source[2] >> 13) & Mask7);
            destination[12] = (int)(((uint)source[2] >> 20) & Mask7);
            destination[13] = (int)((uint)source[2] >> 27) | ((source[3] & Mask2) << (7 - 2));
            destination[14] = (int)(((uint)source[3] >> 2) & Mask7);
            destination[15] = (int)(((uint)source[3] >> 9) & Mask7);
            destination[16] = (int)(((uint)source[3] >> 16) & Mask7);
            destination[17] = (int)(((uint)source[3] >> 23) & Mask7);
            destination[18] = (int)((uint)source[3] >> 30) | ((source[4] & Mask5) << (7 - 5));
            destination[19] = (int)(((uint)source[4] >> 5) & Mask7);
            destination[20] = (int)(((uint)source[4] >> 12) & Mask7);
            destination[21] = (int)(((uint)source[4] >> 19) & Mask7);
            destination[22] = (int)((uint)source[4] >> 26) | ((source[5] & Mask1) << (7 - 1));
            destination[23] = (int)(((uint)source[5] >> 1) & Mask7);
            destination[24] = (int)(((uint)source[5] >> 8) & Mask7);
            destination[25] = (int)(((uint)source[5] >> 15) & Mask7);
            destination[26] = (int)(((uint)source[5] >> 22) & Mask7);
            destination[27] = (int)((uint)source[5] >> 29) | ((source[6] & Mask4) << (7 - 4));
            destination[28] = (int)(((uint)source[6] >> 4) & Mask7);
            destination[29] = (int)(((uint)source[6] >> 11) & Mask7);
            destination[30] = (int)(((uint)source[6] >> 18) & Mask7);
            destination[31] = (int)((uint)source[6] >> 25);
        }

        static void Unpack8(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask8);
            destination[1] = (int)(((uint)source[0] >> 8) & Mask8);
            destination[2] = (int)(((uint)source[0] >> 16) & Mask8);
            destination[3] = (int)((uint)source[0] >> 24);
            destination[4] = (int)(((uint)source[1] >> 0) & Mask8);
            destination[5] = (int)(((uint)source[1] >> 8) & Mask8);
            destination[6] = (int)(((uint)source[1] >> 16) & Mask8);
            destination[7] = (int)((uint)source[1] >> 24);
            destination[8] = (int)(((uint)source[2] >> 0) & Mask8);
            destination[9] = (int)(((uint)source[2] >> 8) & Mask8);
            destination[10] = (int)(((uint)source[2] >> 16) & Mask8);
            destination[11] = (int)((uint)source[2] >> 24);
            destination[12] = (int)(((uint)source[3] >> 0) & Mask8);
            destination[13] = (int)(((uint)source[3] >> 8) & Mask8);
            destination[14] = (int)(((uint)source[3] >> 16) & Mask8);
            destination[15] = (int)((uint)source[3] >> 24);
            destination[16] = (int)(((uint)source[4] >> 0) & Mask8);
            destination[17] = (int)(((uint)source[4] >> 8) & Mask8);
            destination[18] = (int)(((uint)source[4] >> 16) & Mask8);
            destination[19] = (int)((uint)source[4] >> 24);
            destination[20] = (int)(((uint)source[5] >> 0) & Mask8);
            destination[21] = (int)(((uint)source[5] >> 8) & Mask8);
            destination[22] = (int)(((uint)source[5] >> 16) & Mask8);
            destination[23] = (int)((uint)source[5] >> 24);
            destination[24] = (int)(((uint)source[6] >> 0) & Mask8);
            destination[25] = (int)(((uint)source[6] >> 8) & Mask8);
            destination[26] = (int)(((uint)source[6] >> 16) & Mask8);
            destination[27] = (int)((uint)source[6] >> 24);
            destination[28] = (int)(((uint)source[7] >> 0) & Mask8);
            destination[29] = (int)(((uint)source[7] >> 8) & Mask8);
            destination[30] = (int)(((uint)source[7] >> 16) & Mask8);
            destination[31] = (int)((uint)source[7] >> 24);
        }

        static void Unpack9(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask9);
            destination[1] = (int)(((uint)source[0] >> 9) & Mask9);
            destination[2] = (int)(((uint)source[0] >> 18) & Mask9);
            destination[3] = (int)((uint)source[0] >> 27) | ((source[1] & Mask4) << (9 - 4));
            destination[4] = (int)(((uint)source[1] >> 4) & Mask9);
            destination[5] = (int)(((uint)source[1] >> 13) & Mask9);
            destination[6] = (int)(((uint)source[1] >> 22) & Mask9);
            destination[7] = (int)((uint)source[1] >> 31) | ((source[2] & Mask8) << (9 - 8));
            destination[8] = (int)(((uint)source[2] >> 8) & Mask9);
            destination[9] = (int)(((uint)source[2] >> 17) & Mask9);
            destination[10] = (int)((uint)source[2] >> 26) | ((source[3] & Mask3) << (9 - 3));
            destination[11] = (int)(((uint)source[3] >> 3) & Mask9);
            destination[12] = (int)(((uint)source[3] >> 12) & Mask9);
            destination[13] = (int)(((uint)source[3] >> 21) & Mask9);
            destination[14] = (int)((uint)source[3] >> 30) | ((source[4] & Mask7) << (9 - 7));
            destination[15] = (int)(((uint)source[4] >> 7) & Mask9);
            destination[16] = (int)(((uint)source[4] >> 16) & Mask9);
            destination[17] = (int)((uint)source[4] >> 25) | ((source[5] & Mask2) << (9 - 2));
            destination[18] = (int)(((uint)source[5] >> 2) & Mask9);
            destination[19] = (int)(((uint)source[5] >> 11) & Mask9);
            destination[20] = (int)(((uint)source[5] >> 20) & Mask9);
            destination[21] = (int)((uint)source[5] >> 29) | ((source[6] & Mask6) << (9 - 6));
            destination[22] = (int)(((uint)source[6] >> 6) & Mask9);
            destination[23] = (int)(((uint)source[6] >> 15) & Mask9);
            destination[24] = (int)((uint)source[6] >> 24) | ((source[7] & Mask1) << (9 - 1));
            destination[25] = (int)(((uint)source[7] >> 1) & Mask9);
            destination[26] = (int)(((uint)source[7] >> 10) & Mask9);
            destination[27] = (int)(((uint)source[7] >> 19) & Mask9);
            destination[28] = (int)((uint)source[7] >> 28) | ((source[8] & Mask5) << (9 - 5));
            destination[29] = (int)(((uint)source[8] >> 5) & Mask9);
            destination[30] = (int)(((uint)source[8] >> 14) & Mask9);
            destination[31] = (int)((uint)source[8] >> 23);
        }

        static void Unpack10(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask10);
            destination[1] = (int)(((uint)source[0] >> 10) & Mask10);
            destination[2] = (int)(((uint)source[0] >> 20) & Mask10);
            destination[3] = (int)((uint)source[0] >> 30) | ((source[1] & Mask8) << (10 - 8));
            destination[4] = (int)(((uint)source[1] >> 8) & Mask10);
            destination[5] = (int)(((uint)source[1] >> 18) & Mask10);
            destination[6] = (int)((uint)source[1] >> 28) | ((source[2] & Mask6) << (10 - 6));
            destination[7] = (int)(((uint)source[2] >> 6) & Mask10);
            destination[8] = (int)(((uint)source[2] >> 16) & Mask10);
            destination[9] = (int)((uint)source[2] >> 26) | ((source[3] & Mask4) << (10 - 4));
            destination[10] = (int)(((uint)source[3] >> 4) & Mask10);
            destination[11] = (int)(((uint)source[3] >> 14) & Mask10);
            destination[12] = (int)((uint)source[3] >> 24) | ((source[4] & Mask2) << (10 - 2));
            destination[13] = (int)(((uint)source[4] >> 2) & Mask10);
            destination[14] = (int)(((uint)source[4] >> 12) & Mask10);
            destination[15] = (int)((uint)source[4] >> 22);
            destination[16] = (int)(((uint)source[5] >> 0) & Mask10);
            destination[17] = (int)(((uint)source[5] >> 10) & Mask10);
            destination[18] = (int)(((uint)source[5] >> 20) & Mask10);
            destination[19] = (int)((uint)source[5] >> 30) | ((source[6] & Mask8) << (10 - 8));
            destination[20] = (int)(((uint)source[6] >> 8) & Mask10);
            destination[21] = (int)(((uint)source[6] >> 18) & Mask10);
            destination[22] = (int)((uint)source[6] >> 28) | ((source[7] & Mask6) << (10 - 6));
            destination[23] = (int)(((uint)source[7] >> 6) & Mask10);
            destination[24] = (int)(((uint)source[7] >> 16) & Mask10);
            destination[25] = (int)((uint)source[7] >> 26) | ((source[8] & Mask4) << (10 - 4));
            destination[26] = (int)(((uint)source[8] >> 4) & Mask10);
            destination[27] = (int)(((uint)source[8] >> 14) & Mask10);
            destination[28] = (int)((uint)source[8] >> 24) | ((source[9] & Mask2) << (10 - 2));
            destination[29] = (int)(((uint)source[9] >> 2) & Mask10);
            destination[30] = (int)(((uint)source[9] >> 12) & Mask10);
            destination[31] = (int)((uint)source[9] >> 22);
        }

        static void Unpack11(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask11);
            destination[1] = (int)(((uint)source[0] >> 11) & Mask11);
            destination[2] = (int)((uint)source[0] >> 22) | ((source[1] & Mask1) << (11 - 1));
            destination[3] = (int)(((uint)source[1] >> 1) & Mask11);
            destination[4] = (int)(((uint)source[1] >> 12) & Mask11);
            destination[5] = (int)((uint)source[1] >> 23) | ((source[2] & Mask2) << (11 - 2));
            destination[6] = (int)(((uint)source[2] >> 2) & Mask11);
            destination[7] = (int)(((uint)source[2] >> 13) & Mask11);
            destination[8] = (int)((uint)source[2] >> 24) | ((source[3] & Mask3) << (11 - 3));
            destination[9] = (int)(((uint)source[3] >> 3) & Mask11);
            destination[10] = (int)(((uint)source[3] >> 14) & Mask11);
            destination[11] = (int)((uint)source[3] >> 25) | ((source[4] & Mask4) << (11 - 4));
            destination[12] = (int)(((uint)source[4] >> 4) & Mask11);
            destination[13] = (int)(((uint)source[4] >> 15) & Mask11);
            destination[14] = (int)((uint)source[4] >> 26) | ((source[5] & Mask5) << (11 - 5));
            destination[15] = (int)(((uint)source[5] >> 5) & Mask11);
            destination[16] = (int)(((uint)source[5] >> 16) & Mask11);
            destination[17] = (int)((uint)source[5] >> 27) | ((source[6] & Mask6) << (11 - 6));
            destination[18] = (int)(((uint)source[6] >> 6) & Mask11);
            destination[19] = (int)(((uint)source[6] >> 17) & Mask11);
            destination[20] = (int)((uint)source[6] >> 28) | ((source[7] & Mask7) << (11 - 7));
            destination[21] = (int)(((uint)source[7] >> 7) & Mask11);
            destination[22] = (int)(((uint)source[7] >> 18) & Mask11);
            destination[23] = (int)((uint)source[7] >> 29) | ((source[8] & Mask8) << (11 - 8));
            destination[24] = (int)(((uint)source[8] >> 8) & Mask11);
            destination[25] = (int)(((uint)source[8] >> 19) & Mask11);
            destination[26] = (int)((uint)source[8] >> 30) | ((source[9] & Mask9) << (11 - 9));
            destination[27] = (int)(((uint)source[9] >> 9) & Mask11);
            destination[28] = (int)(((uint)source[9] >> 20) & Mask11);
            destination[29] = (int)((uint)source[9] >> 31) | ((source[10] & Mask10) << (11 - 10));
            destination[30] = (int)(((uint)source[10] >> 10) & Mask11);
            destination[31] = (int)((uint)source[10] >> 21);
        }

        static void Unpack12(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask12);
            destination[1] = (int)(((uint)source[0] >> 12) & Mask12);
            destination[2] = (int)((uint)source[0] >> 24) | ((source[1] & Mask4) << (12 - 4));
            destination[3] = (int)(((uint)source[1] >> 4) & Mask12);
            destination[4] = (int)(((uint)source[1] >> 16) & Mask12);
            destination[5] = (int)((uint)source[1] >> 28) | ((source[2] & Mask8) << (12 - 8));
            destination[6] = (int)(((uint)source[2] >> 8) & Mask12);
            destination[7] = (int)((uint)source[2] >> 20);
            destination[8] = (int)(((uint)source[3] >> 0) & Mask12);
            destination[9] = (int)(((uint)source[3] >> 12) & Mask12);
            destination[10] = (int)((uint)source[3] >> 24) | ((source[4] & Mask4) << (12 - 4));
            destination[11] = (int)(((uint)source[4] >> 4) & Mask12);
            destination[12] = (int)(((uint)source[4] >> 16) & Mask12);
            destination[13] = (int)((uint)source[4] >> 28) | ((source[5] & Mask8) << (12 - 8));
            destination[14] = (int)(((uint)source[5] >> 8) & Mask12);
            destination[15] = (int)((uint)source[5] >> 20);
            destination[16] = (int)(((uint)source[6] >> 0) & Mask12);
            destination[17] = (int)(((uint)source[6] >> 12) & Mask12);
            destination[18] = (int)((uint)source[6] >> 24) | ((source[7] & Mask4) << (12 - 4));
            destination[19] = (int)(((uint)source[7] >> 4) & Mask12);
            destination[20] = (int)(((uint)source[7] >> 16) & Mask12);
            destination[21] = (int)((uint)source[7] >> 28) | ((source[8] & Mask8) << (12 - 8));
            destination[22] = (int)(((uint)source[8] >> 8) & Mask12);
            destination[23] = (int)((uint)source[8] >> 20);
            destination[24] = (int)(((uint)source[9] >> 0) & Mask12);
            destination[25] = (int)(((uint)source[9] >> 12) & Mask12);
            destination[26] = (int)((uint)source[9] >> 24) | ((source[10] & Mask4) << (12 - 4));
            destination[27] = (int)(((uint)source[10] >> 4) & Mask12);
            destination[28] = (int)(((uint)source[10] >> 16) & Mask12);
            destination[29] = (int)((uint)source[10] >> 28) | ((source[11] & Mask8) << (12 - 8));
            destination[30] = (int)(((uint)source[11] >> 8) & Mask12);
            destination[31] = (int)((uint)source[11] >> 20);
        }

        static void Unpack13(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask13);
            destination[1] = (int)(((uint)source[0] >> 13) & Mask13);
            destination[2] = (int)((uint)source[0] >> 26) | ((source[1] & Mask7) << (13 - 7));
            destination[3] = (int)(((uint)source[1] >> 7) & Mask13);
            destination[4] = (int)((uint)source[1] >> 20) | ((source[2] & Mask1) << (13 - 1));
            destination[5] = (int)(((uint)source[2] >> 1) & Mask13);
            destination[6] = (int)(((uint)source[2] >> 14) & Mask13);
            destination[7] = (int)((uint)source[2] >> 27) | ((source[3] & Mask8) << (13 - 8));
            destination[8] = (int)(((uint)source[3] >> 8) & Mask13);
            destination[9] = (int)((uint)source[3] >> 21) | ((source[4] & Mask2) << (13 - 2));
            destination[10] = (int)(((uint)source[4] >> 2) & Mask13);
            destination[11] = (int)(((uint)source[4] >> 15) & Mask13);
            destination[12] = (int)((uint)source[4] >> 28) | ((source[5] & Mask9) << (13 - 9));
            destination[13] = (int)(((uint)source[5] >> 9) & Mask13);
            destination[14] = (int)((uint)source[5] >> 22) | ((source[6] & Mask3) << (13 - 3));
            destination[15] = (int)(((uint)source[6] >> 3) & Mask13);
            destination[16] = (int)(((uint)source[6] >> 16) & Mask13);
            destination[17] = (int)((uint)source[6] >> 29) | ((source[7] & Mask10) << (13 - 10));
            destination[18] = (int)(((uint)source[7] >> 10) & Mask13);
            destination[19] = (int)((uint)source[7] >> 23) | ((source[8] & Mask4) << (13 - 4));
            destination[20] = (int)(((uint)source[8] >> 4) & Mask13);
            destination[21] = (int)(((uint)source[8] >> 17) & Mask13);
            destination[22] = (int)((uint)source[8] >> 30) | ((source[9] & Mask11) << (13 - 11));
            destination[23] = (int)(((uint)source[9] >> 11) & Mask13);
            destination[24] = (int)((uint)source[9] >> 24) | ((source[10] & Mask5) << (13 - 5));
            destination[25] = (int)(((uint)source[10] >> 5) & Mask13);
            destination[26] = (int)(((uint)source[10] >> 18) & Mask13);
            destination[27] = (int)((uint)source[10] >> 31) | ((source[11] & Mask12) << (13 - 12));
            destination[28] = (int)(((uint)source[11] >> 12) & Mask13);
            destination[29] = (int)((uint)source[11] >> 25) | ((source[12] & Mask6) << (13 - 6));
            destination[30] = (int)(((uint)source[12] >> 6) & Mask13);
            destination[31] = (int)((uint)source[12] >> 19);
        }

        static void Unpack14(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask14);
            destination[1] = (int)(((uint)source[0] >> 14) & Mask14);
            destination[2] = (int)((uint)source[0] >> 28) | ((source[1] & Mask10) << (14 - 10));
            destination[3] = (int)(((uint)source[1] >> 10) & Mask14);
            destination[4] = (int)((uint)source[1] >> 24) | ((source[2] & Mask6) << (14 - 6));
            destination[5] = (int)(((uint)source[2] >> 6) & Mask14);
            destination[6] = (int)((uint)source[2] >> 20) | ((source[3] & Mask2) << (14 - 2));
            destination[7] = (int)(((uint)source[3] >> 2) & Mask14);
            destination[8] = (int)(((uint)source[3] >> 16) & Mask14);
            destination[9] = (int)((uint)source[3] >> 30) | ((source[4] & Mask12) << (14 - 12));
            destination[10] = (int)(((uint)source[4] >> 12) & Mask14);
            destination[11] = (int)((uint)source[4] >> 26) | ((source[5] & Mask8) << (14 - 8));
            destination[12] = (int)(((uint)source[5] >> 8) & Mask14);
            destination[13] = (int)((uint)source[5] >> 22) | ((source[6] & Mask4) << (14 - 4));
            destination[14] = (int)(((uint)source[6] >> 4) & Mask14);
            destination[15] = (int)((uint)source[6] >> 18);
            destination[16] = (int)(((uint)source[7] >> 0) & Mask14);
            destination[17] = (int)(((uint)source[7] >> 14) & Mask14);
            destination[18] = (int)((uint)source[7] >> 28) | ((source[8] & Mask10) << (14 - 10));
            destination[19] = (int)(((uint)source[8] >> 10) & Mask14);
            destination[20] = (int)((uint)source[8] >> 24) | ((source[9] & Mask6) << (14 - 6));
            destination[21] = (int)(((uint)source[9] >> 6) & Mask14);
            destination[22] = (int)((uint)source[9] >> 20) | ((source[10] & Mask2) << (14 - 2));
            destination[23] = (int)(((uint)source[10] >> 2) & Mask14);
            destination[24] = (int)(((uint)source[10] >> 16) & Mask14);
            destination[25] = (int)((uint)source[10] >> 30) | ((source[11] & Mask12) << (14 - 12));
            destination[26] = (int)(((uint)source[11] >> 12) & Mask14);
            destination[27] = (int)((uint)source[11] >> 26) | ((source[12] & Mask8) << (14 - 8));
            destination[28] = (int)(((uint)source[12] >> 8) & Mask14);
            destination[29] = (int)((uint)source[12] >> 22) | ((source[13] & Mask4) << (14 - 4));
            destination[30] = (int)(((uint)source[13] >> 4) & Mask14);
            destination[31] = (int)((uint)source[13] >> 18);
        }

        static void Unpack15(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask15);
            destination[1] = (int)(((uint)source[0] >> 15) & Mask15);
            destination[2] = (int)((uint)source[0] >> 30) | ((source[1] & Mask13) << (15 - 13));
            destination[3] = (int)(((uint)source[1] >> 13) & Mask15);
            destination[4] = (int)((uint)source[1] >> 28) | ((source[2] & Mask11) << (15 - 11));
            destination[5] = (int)(((uint)source[2] >> 11) & Mask15);
            destination[6] = (int)((uint)source[2] >> 26) | ((source[3] & Mask9) << (15 - 9));
            destination[7] = (int)(((uint)source[3] >> 9) & Mask15);
            destination[8] = (int)((uint)source[3] >> 24) | ((source[4] & Mask7) << (15 - 7));
            destination[9] = (int)(((uint)source[4] >> 7) & Mask15);
            destination[10] = (int)((uint)source[4] >> 22) | ((source[5] & Mask5) << (15 - 5));
            destination[11] = (int)(((uint)source[5] >> 5) & Mask15);
            destination[12] = (int)((uint)source[5] >> 20) | ((source[6] & Mask3) << (15 - 3));
            destination[13] = (int)(((uint)source[6] >> 3) & Mask15);
            destination[14] = (int)((uint)source[6] >> 18) | ((source[7] & Mask1) << (15 - 1));
            destination[15] = (int)(((uint)source[7] >> 1) & Mask15);
            destination[16] = (int)(((uint)source[7] >> 16) & Mask15);
            destination[17] = (int)((uint)source[7] >> 31) | ((source[8] & Mask14) << (15 - 14));
            destination[18] = (int)(((uint)source[8] >> 14) & Mask15);
            destination[19] = (int)((uint)source[8] >> 29) | ((source[9] & Mask12) << (15 - 12));
            destination[20] = (int)(((uint)source[9] >> 12) & Mask15);
            destination[21] = (int)((uint)source[9] >> 27) | ((source[10] & Mask10) << (15 - 10));
            destination[22] = (int)(((uint)source[10] >> 10) & Mask15);
            destination[23] = (int)((uint)source[10] >> 25) | ((source[11] & Mask8) << (15 - 8));
            destination[24] = (int)(((uint)source[11] >> 8) & Mask15);
            destination[25] = (int)((uint)source[11] >> 23) | ((source[12] & Mask6) << (15 - 6));
            destination[26] = (int)(((uint)source[12] >> 6) & Mask15);
            destination[27] = (int)((uint)source[12] >> 21) | ((source[13] & Mask4) << (15 - 4));
            destination[28] = (int)(((uint)source[13] >> 4) & Mask15);
            destination[29] = (int)((uint)source[13] >> 19) | ((source[14] & Mask2) << (15 - 2));
            destination[30] = (int)(((uint)source[14] >> 2) & Mask15);
            destination[31] = (int)((uint)source[14] >> 17);
        }

        static void Unpack16(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask16);
            destination[1] = (int)((uint)source[0] >> 16);
            destination[2] = (int)(((uint)source[1] >> 0) & Mask16);
            destination[3] = (int)((uint)source[1] >> 16);
            destination[4] = (int)(((uint)source[2] >> 0) & Mask16);
            destination[5] = (int)((uint)source[2] >> 16);
            destination[6] = (int)(((uint)source[3] >> 0) & Mask16);
            destination[7] = (int)((uint)source[3] >> 16);
            destination[8] = (int)(((uint)source[4] >> 0) & Mask16);
            destination[9] = (int)((uint)source[4] >> 16);
            destination[10] = (int)(((uint)source[5] >> 0) & Mask16);
            destination[11] = (int)((uint)source[5] >> 16);
            destination[12] = (int)(((uint)source[6] >> 0) & Mask16);
            destination[13] = (int)((uint)source[6] >> 16);
            destination[14] = (int)(((uint)source[7] >> 0) & Mask16);
            destination[15] = (int)((uint)source[7] >> 16);
            destination[16] = (int)(((uint)source[8] >> 0) & Mask16);
            destination[17] = (int)((uint)source[8] >> 16);
            destination[18] = (int)(((uint)source[9] >> 0) & Mask16);
            destination[19] = (int)((uint)source[9] >> 16);
            destination[20] = (int)(((uint)source[10] >> 0) & Mask16);
            destination[21] = (int)((uint)source[10] >> 16);
            destination[22] = (int)(((uint)source[11] >> 0) & Mask16);
            destination[23] = (int)((uint)source[11] >> 16);
            destination[24] = (int)(((uint)source[12] >> 0) & Mask16);
            destination[25] = (int)((uint)source[12] >> 16);
            destination[26] = (int)(((uint)source[13] >> 0) & Mask16);
            destination[27] = (int)((uint)source[13] >> 16);
            destination[28] = (int)(((uint)source[14] >> 0) & Mask16);
            destination[29] = (int)((uint)source[14] >> 16);
            destination[30] = (int)(((uint)source[15] >> 0) & Mask16);
            destination[31] = (int)((uint)source[15] >> 16);
        }

        static void Unpack17(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask17);
            destination[1] = (int)((uint)source[0] >> 17) | ((source[1] & Mask2) << (17 - 2));
            destination[2] = (int)(((uint)source[1] >> 2) & Mask17);
            destination[3] = (int)((uint)source[1] >> 19) | ((source[2] & Mask4) << (17 - 4));
            destination[4] = (int)(((uint)source[2] >> 4) & Mask17);
            destination[5] = (int)((uint)source[2] >> 21) | ((source[3] & Mask6) << (17 - 6));
            destination[6] = (int)(((uint)source[3] >> 6) & Mask17);
            destination[7] = (int)((uint)source[3] >> 23) | ((source[4] & Mask8) << (17 - 8));
            destination[8] = (int)(((uint)source[4] >> 8) & Mask17);
            destination[9] = (int)((uint)source[4] >> 25) | ((source[5] & Mask10) << (17 - 10));
            destination[10] = (int)(((uint)source[5] >> 10) & Mask17);
            destination[11] = (int)((uint)source[5] >> 27) | ((source[6] & Mask12) << (17 - 12));
            destination[12] = (int)(((uint)source[6] >> 12) & Mask17);
            destination[13] = (int)((uint)source[6] >> 29) | ((source[7] & Mask14) << (17 - 14));
            destination[14] = (int)(((uint)source[7] >> 14) & Mask17);
            destination[15] = (int)((uint)source[7] >> 31) | ((source[8] & Mask16) << (17 - 16));
            destination[16] = (int)((uint)source[8] >> 16) | ((source[9] & Mask1) << (17 - 1));
            destination[17] = (int)(((uint)source[9] >> 1) & Mask17);
            destination[18] = (int)((uint)source[9] >> 18) | ((source[10] & Mask3) << (17 - 3));
            destination[19] = (int)(((uint)source[10] >> 3) & Mask17);
            destination[20] = (int)((uint)source[10] >> 20) | ((source[11] & Mask5) << (17 - 5));
            destination[21] = (int)(((uint)source[11] >> 5) & Mask17);
            destination[22] = (int)((uint)source[11] >> 22) | ((source[12] & Mask7) << (17 - 7));
            destination[23] = (int)(((uint)source[12] >> 7) & Mask17);
            destination[24] = (int)((uint)source[12] >> 24) | ((source[13] & Mask9) << (17 - 9));
            destination[25] = (int)(((uint)source[13] >> 9) & Mask17);
            destination[26] = (int)((uint)source[13] >> 26) | ((source[14] & Mask11) << (17 - 11));
            destination[27] = (int)(((uint)source[14] >> 11) & Mask17);
            destination[28] = (int)((uint)source[14] >> 28) | ((source[15] & Mask13) << (17 - 13));
            destination[29] = (int)(((uint)source[15] >> 13) & Mask17);
            destination[30] = (int)((uint)source[15] >> 30) | ((source[16] & Mask15) << (17 - 15));
            destination[31] = (int)((uint)source[16] >> 15);
        }

        static void Unpack18(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask18);
            destination[1] = (int)((uint)source[0] >> 18) | ((source[1] & Mask4) << (18 - 4));
            destination[2] = (int)(((uint)source[1] >> 4) & Mask18);
            destination[3] = (int)((uint)source[1] >> 22) | ((source[2] & Mask8) << (18 - 8));
            destination[4] = (int)(((uint)source[2] >> 8) & Mask18);
            destination[5] = (int)((uint)source[2] >> 26) | ((source[3] & Mask12) << (18 - 12));
            destination[6] = (int)(((uint)source[3] >> 12) & Mask18);
            destination[7] = (int)((uint)source[3] >> 30) | ((source[4] & Mask16) << (18 - 16));
            destination[8] = (int)((uint)source[4] >> 16) | ((source[5] & Mask2) << (18 - 2));
            destination[9] = (int)(((uint)source[5] >> 2) & Mask18);
            destination[10] = (int)((uint)source[5] >> 20) | ((source[6] & Mask6) << (18 - 6));
            destination[11] = (int)(((uint)source[6] >> 6) & Mask18);
            destination[12] = (int)((uint)source[6] >> 24) | ((source[7] & Mask10) << (18 - 10));
            destination[13] = (int)(((uint)source[7] >> 10) & Mask18);
            destination[14] = (int)((uint)source[7] >> 28) | ((source[8] & Mask14) << (18 - 14));
            destination[15] = (int)((uint)source[8] >> 14);
            destination[16] = (int)(((uint)source[9] >> 0) & Mask18);
            destination[17] = (int)((uint)source[9] >> 18) | ((source[10] & Mask4) << (18 - 4));
            destination[18] = (int)(((uint)source[10] >> 4) & Mask18);
            destination[19] = (int)((uint)source[10] >> 22) | ((source[11] & Mask8) << (18 - 8));
            destination[20] = (int)(((uint)source[11] >> 8) & Mask18);
            destination[21] = (int)((uint)source[11] >> 26) | ((source[12] & Mask12) << (18 - 12));
            destination[22] = (int)(((uint)source[12] >> 12) & Mask18);
            destination[23] = (int)((uint)source[12] >> 30) | ((source[13] & Mask16) << (18 - 16));
            destination[24] = (int)((uint)source[13] >> 16) | ((source[14] & Mask2) << (18 - 2));
            destination[25] = (int)(((uint)source[14] >> 2) & Mask18);
            destination[26] = (int)((uint)source[14] >> 20) | ((source[15] & Mask6) << (18 - 6));
            destination[27] = (int)(((uint)source[15] >> 6) & Mask18);
            destination[28] = (int)((uint)source[15] >> 24) | ((source[16] & Mask10) << (18 - 10));
            destination[29] = (int)(((uint)source[16] >> 10) & Mask18);
            destination[30] = (int)((uint)source[16] >> 28) | ((source[17] & Mask14) << (18 - 14));
            destination[31] = (int)((uint)source[17] >> 14);
        }

        static void Unpack19(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask19);
            destination[1] = (int)((uint)source[0] >> 19) | ((source[1] & Mask6) << (19 - 6));
            destination[2] = (int)(((uint)source[1] >> 6) & Mask19);
            destination[3] = (int)((uint)source[1] >> 25) | ((source[2] & Mask12) << (19 - 12));
            destination[4] = (int)(((uint)source[2] >> 12) & Mask19);
            destination[5] = (int)((uint)source[2] >> 31) | ((source[3] & Mask18) << (19 - 18));
            destination[6] = (int)((uint)source[3] >> 18) | ((source[4] & Mask5) << (19 - 5));
            destination[7] = (int)(((uint)source[4] >> 5) & Mask19);
            destination[8] = (int)((uint)source[4] >> 24) | ((source[5] & Mask11) << (19 - 11));
            destination[9] = (int)(((uint)source[5] >> 11) & Mask19);
            destination[10] = (int)((uint)source[5] >> 30) | ((source[6] & Mask17) << (19 - 17));
            destination[11] = (int)((uint)source[6] >> 17) | ((source[7] & Mask4) << (19 - 4));
            destination[12] = (int)(((uint)source[7] >> 4) & Mask19);
            destination[13] = (int)((uint)source[7] >> 23) | ((source[8] & Mask10) << (19 - 10));
            destination[14] = (int)(((uint)source[8] >> 10) & Mask19);
            destination[15] = (int)((uint)source[8] >> 29) | ((source[9] & Mask16) << (19 - 16));
            destination[16] = (int)((uint)source[9] >> 16) | ((source[10] & Mask3) << (19 - 3));
            destination[17] = (int)(((uint)source[10] >> 3) & Mask19);
            destination[18] = (int)((uint)source[10] >> 22) | ((source[11] & Mask9) << (19 - 9));
            destination[19] = (int)(((uint)source[11] >> 9) & Mask19);
            destination[20] = (int)((uint)source[11] >> 28) | ((source[12] & Mask15) << (19 - 15));
            destination[21] = (int)((uint)source[12] >> 15) | ((source[13] & Mask2) << (19 - 2));
            destination[22] = (int)(((uint)source[13] >> 2) & Mask19);
            destination[23] = (int)((uint)source[13] >> 21) | ((source[14] & Mask8) << (19 - 8));
            destination[24] = (int)(((uint)source[14] >> 8) & Mask19);
            destination[25] = (int)((uint)source[14] >> 27) | ((source[15] & Mask14) << (19 - 14));
            destination[26] = (int)((uint)source[15] >> 14) | ((source[16] & Mask1) << (19 - 1));
            destination[27] = (int)(((uint)source[16] >> 1) & Mask19);
            destination[28] = (int)((uint)source[16] >> 20) | ((source[17] & Mask7) << (19 - 7));
            destination[29] = (int)(((uint)source[17] >> 7) & Mask19);
            destination[30] = (int)((uint)source[17] >> 26) | ((source[18] & Mask13) << (19 - 13));
            destination[31] = (int)((uint)source[18] >> 13);
        }

        static void Unpack20(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask20);
            destination[1] = (int)((uint)source[0] >> 20) | ((source[1] & Mask8) << (20 - 8));
            destination[2] = (int)(((uint)source[1] >> 8) & Mask20);
            destination[3] = (int)((uint)source[1] >> 28) | ((source[2] & Mask16) << (20 - 16));
            destination[4] = (int)((uint)source[2] >> 16) | ((source[3] & Mask4) << (20 - 4));
            destination[5] = (int)(((uint)source[3] >> 4) & Mask20);
            destination[6] = (int)((uint)source[3] >> 24) | ((source[4] & Mask12) << (20 - 12));
            destination[7] = (int)((uint)source[4] >> 12);
            destination[8] = (int)(((uint)source[5] >> 0) & Mask20);
            destination[9] = (int)((uint)source[5] >> 20) | ((source[6] & Mask8) << (20 - 8));
            destination[10] = (int)(((uint)source[6] >> 8) & Mask20);
            destination[11] = (int)((uint)source[6] >> 28) | ((source[7] & Mask16) << (20 - 16));
            destination[12] = (int)((uint)source[7] >> 16) | ((source[8] & Mask4) << (20 - 4));
            destination[13] = (int)(((uint)source[8] >> 4) & Mask20);
            destination[14] = (int)((uint)source[8] >> 24) | ((source[9] & Mask12) << (20 - 12));
            destination[15] = (int)((uint)source[9] >> 12);
            destination[16] = (int)(((uint)source[10] >> 0) & Mask20);
            destination[17] = (int)((uint)source[10] >> 20) | ((source[11] & Mask8) << (20 - 8));
            destination[18] = (int)(((uint)source[11] >> 8) & Mask20);
            destination[19] = (int)((uint)source[11] >> 28) | ((source[12] & Mask16) << (20 - 16));
            destination[20] = (int)((uint)source[12] >> 16) | ((source[13] & Mask4) << (20 - 4));
            destination[21] = (int)(((uint)source[13] >> 4) & Mask20);
            destination[22] = (int)((uint)source[13] >> 24) | ((source[14] & Mask12) << (20 - 12));
            destination[23] = (int)((uint)source[14] >> 12);
            destination[24] = (int)(((uint)source[15] >> 0) & Mask20);
            destination[25] = (int)((uint)source[15] >> 20) | ((source[16] & Mask8) << (20 - 8));
            destination[26] = (int)(((uint)source[16] >> 8) & Mask20);
            destination[27] = (int)((uint)source[16] >> 28) | ((source[17] & Mask16) << (20 - 16));
            destination[28] = (int)((uint)source[17] >> 16) | ((source[18] & Mask4) << (20 - 4));
            destination[29] = (int)(((uint)source[18] >> 4) & Mask20);
            destination[30] = (int)((uint)source[18] >> 24) | ((source[19] & Mask12) << (20 - 12));
            destination[31] = (int)((uint)source[19] >> 12);
        }

        static void Unpack21(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask21);
            destination[1] = (int)((uint)source[0] >> 21) | ((source[1] & Mask10) << (21 - 10));
            destination[2] = (int)(((uint)source[1] >> 10) & Mask21);
            destination[3] = (int)((uint)source[1] >> 31) | ((source[2] & Mask20) << (21 - 20));
            destination[4] = (int)((uint)source[2] >> 20) | ((source[3] & Mask9) << (21 - 9));
            destination[5] = (int)(((uint)source[3] >> 9) & Mask21);
            destination[6] = (int)((uint)source[3] >> 30) | ((source[4] & Mask19) << (21 - 19));
            destination[7] = (int)((uint)source[4] >> 19) | ((source[5] & Mask8) << (21 - 8));
            destination[8] = (int)(((uint)source[5] >> 8) & Mask21);
            destination[9] = (int)((uint)source[5] >> 29) | ((source[6] & Mask18) << (21 - 18));
            destination[10] = (int)((uint)source[6] >> 18) | ((source[7] & Mask7) << (21 - 7));
            destination[11] = (int)(((uint)source[7] >> 7) & Mask21);
            destination[12] = (int)((uint)source[7] >> 28) | ((source[8] & Mask17) << (21 - 17));
            destination[13] = (int)((uint)source[8] >> 17) | ((source[9] & Mask6) << (21 - 6));
            destination[14] = (int)(((uint)source[9] >> 6) & Mask21);
            destination[15] = (int)((uint)source[9] >> 27) | ((source[10] & Mask16) << (21 - 16));
            destination[16] = (int)((uint)source[10] >> 16) | ((source[11] & Mask5) << (21 - 5));
            destination[17] = (int)(((uint)source[11] >> 5) & Mask21);
            destination[18] = (int)((uint)source[11] >> 26) | ((source[12] & Mask15) << (21 - 15));
            destination[19] = (int)((uint)source[12] >> 15) | ((source[13] & Mask4) << (21 - 4));
            destination[20] = (int)(((uint)source[13] >> 4) & Mask21);
            destination[21] = (int)((uint)source[13] >> 25) | ((source[14] & Mask14) << (21 - 14));
            destination[22] = (int)((uint)source[14] >> 14) | ((source[15] & Mask3) << (21 - 3));
            destination[23] = (int)(((uint)source[15] >> 3) & Mask21);
            destination[24] = (int)((uint)source[15] >> 24) | ((source[16] & Mask13) << (21 - 13));
            destination[25] = (int)((uint)source[16] >> 13) | ((source[17] & Mask2) << (21 - 2));
            destination[26] = (int)(((uint)source[17] >> 2) & Mask21);
            destination[27] = (int)((uint)source[17] >> 23) | ((source[18] & Mask12) << (21 - 12));
            destination[28] = (int)((uint)source[18] >> 12) | ((source[19] & Mask1) << (21 - 1));
            destination[29] = (int)(((uint)source[19] >> 1) & Mask21);
            destination[30] = (int)((uint)source[19] >> 22) | ((source[20] & Mask11) << (21 - 11));
            destination[31] = (int)((uint)source[20] >> 11);
        }

        static void Unpack22(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask22);
            destination[1] = (int)((uint)source[0] >> 22) | ((source[1] & Mask12) << (22 - 12));
            destination[2] = (int)((uint)source[1] >> 12) | ((source[2] & Mask2) << (22 - 2));
            destination[3] = (int)(((uint)source[2] >> 2) & Mask22);
            destination[4] = (int)((uint)source[2] >> 24) | ((source[3] & Mask14) << (22 - 14));
            destination[5] = (int)((uint)source[3] >> 14) | ((source[4] & Mask4) << (22 - 4));
            destination[6] = (int)(((uint)source[4] >> 4) & Mask22);
            destination[7] = (int)((uint)source[4] >> 26) | ((source[5] & Mask16) << (22 - 16));
            destination[8] = (int)((uint)source[5] >> 16) | ((source[6] & Mask6) << (22 - 6));
            destination[9] = (int)(((uint)source[6] >> 6) & Mask22);
            destination[10] = (int)((uint)source[6] >> 28) | ((source[7] & Mask18) << (22 - 18));
            destination[11] = (int)((uint)source[7] >> 18) | ((source[8] & Mask8) << (22 - 8));
            destination[12] = (int)(((uint)source[8] >> 8) & Mask22);
            destination[13] = (int)((uint)source[8] >> 30) | ((source[9] & Mask20) << (22 - 20));
            destination[14] = (int)((uint)source[9] >> 20) | ((source[10] & Mask10) << (22 - 10));
            destination[15] = (int)((uint)source[10] >> 10);
            destination[16] = (int)(((uint)source[11] >> 0) & Mask22);
            destination[17] = (int)((uint)source[11] >> 22) | ((source[12] & Mask12) << (22 - 12));
            destination[18] = (int)((uint)source[12] >> 12) | ((source[13] & Mask2) << (22 - 2));
            destination[19] = (int)(((uint)source[13] >> 2) & Mask22);
            destination[20] = (int)((uint)source[13] >> 24) | ((source[14] & Mask14) << (22 - 14));
            destination[21] = (int)((uint)source[14] >> 14) | ((source[15] & Mask4) << (22 - 4));
            destination[22] = (int)(((uint)source[15] >> 4) & Mask22);
            destination[23] = (int)((uint)source[15] >> 26) | ((source[16] & Mask16) << (22 - 16));
            destination[24] = (int)((uint)source[16] >> 16) | ((source[17] & Mask6) << (22 - 6));
            destination[25] = (int)(((uint)source[17] >> 6) & Mask22);
            destination[26] = (int)((uint)source[17] >> 28) | ((source[18] & Mask18) << (22 - 18));
            destination[27] = (int)((uint)source[18] >> 18) | ((source[19] & Mask8) << (22 - 8));
            destination[28] = (int)(((uint)source[19] >> 8) & Mask22);
            destination[29] = (int)((uint)source[19] >> 30) | ((source[20] & Mask20) << (22 - 20));
            destination[30] = (int)((uint)source[20] >> 20) | ((source[21] & Mask10) << (22 - 10));
            destination[31] = (int)((uint)source[21] >> 10);
        }

        static void Unpack23(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask23);
            destination[1] = (int)((uint)source[0] >> 23) | ((source[1] & Mask14) << (23 - 14));
            destination[2] = (int)((uint)source[1] >> 14) | ((source[2] & Mask5) << (23 - 5));
            destination[3] = (int)(((uint)source[2] >> 5) & Mask23);
            destination[4] = (int)((uint)source[2] >> 28) | ((source[3] & Mask19) << (23 - 19));
            destination[5] = (int)((uint)source[3] >> 19) | ((source[4] & Mask10) << (23 - 10));
            destination[6] = (int)((uint)source[4] >> 10) | ((source[5] & Mask1) << (23 - 1));
            destination[7] = (int)(((uint)source[5] >> 1) & Mask23);
            destination[8] = (int)((uint)source[5] >> 24) | ((source[6] & Mask15) << (23 - 15));
            destination[9] = (int)((uint)source[6] >> 15) | ((source[7] & Mask6) << (23 - 6));
            destination[10] = (int)(((uint)source[7] >> 6) & Mask23);
            destination[11] = (int)((uint)source[7] >> 29) | ((source[8] & Mask20) << (23 - 20));
            destination[12] = (int)((uint)source[8] >> 20) | ((source[9] & Mask11) << (23 - 11));
            destination[13] = (int)((uint)source[9] >> 11) | ((source[10] & Mask2) << (23 - 2));
            destination[14] = (int)(((uint)source[10] >> 2) & Mask23);
            destination[15] = (int)((uint)source[10] >> 25) | ((source[11] & Mask16) << (23 - 16));
            destination[16] = (int)((uint)source[11] >> 16) | ((source[12] & Mask7) << (23 - 7));
            destination[17] = (int)(((uint)source[12] >> 7) & Mask23);
            destination[18] = (int)((uint)source[12] >> 30) | ((source[13] & Mask21) << (23 - 21));
            destination[19] = (int)((uint)source[13] >> 21) | ((source[14] & Mask12) << (23 - 12));
            destination[20] = (int)((uint)source[14] >> 12) | ((source[15] & Mask3) << (23 - 3));
            destination[21] = (int)(((uint)source[15] >> 3) & Mask23);
            destination[22] = (int)((uint)source[15] >> 26) | ((source[16] & Mask17) << (23 - 17));
            destination[23] = (int)((uint)source[16] >> 17) | ((source[17] & Mask8) << (23 - 8));
            destination[24] = (int)(((uint)source[17] >> 8) & Mask23);
            destination[25] = (int)((uint)source[17] >> 31) | ((source[18] & Mask22) << (23 - 22));
            destination[26] = (int)((uint)source[18] >> 22) | ((source[19] & Mask13) << (23 - 13));
            destination[27] = (int)((uint)source[19] >> 13) | ((source[20] & Mask4) << (23 - 4));
            destination[28] = (int)(((uint)source[20] >> 4) & Mask23);
            destination[29] = (int)((uint)source[20] >> 27) | ((source[21] & Mask18) << (23 - 18));
            destination[30] = (int)((uint)source[21] >> 18) | ((source[22] & Mask9) << (23 - 9));
            destination[31] = (int)((uint)source[22] >> 9);
        }

        static void Unpack24(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask24);
            destination[1] = (int)((uint)source[0] >> 24) | ((source[1] & Mask16) << (24 - 16));
            destination[2] = (int)((uint)source[1] >> 16) | ((source[2] & Mask8) << (24 - 8));
            destination[3] = (int)((uint)source[2] >> 8);
            destination[4] = (int)(((uint)source[3] >> 0) & Mask24);
            destination[5] = (int)((uint)source[3] >> 24) | ((source[4] & Mask16) << (24 - 16));
            destination[6] = (int)((uint)source[4] >> 16) | ((source[5] & Mask8) << (24 - 8));
            destination[7] = (int)((uint)source[5] >> 8);
            destination[8] = (int)(((uint)source[6] >> 0) & Mask24);
            destination[9] = (int)((uint)source[6] >> 24) | ((source[7] & Mask16) << (24 - 16));
            destination[10] = (int)((uint)source[7] >> 16) | ((source[8] & Mask8) << (24 - 8));
            destination[11] = (int)((uint)source[8] >> 8);
            destination[12] = (int)(((uint)source[9] >> 0) & Mask24);
            destination[13] = (int)((uint)source[9] >> 24) | ((source[10] & Mask16) << (24 - 16));
            destination[14] = (int)((uint)source[10] >> 16) | ((source[11] & Mask8) << (24 - 8));
            destination[15] = (int)((uint)source[11] >> 8);
            destination[16] = (int)(((uint)source[12] >> 0) & Mask24);
            destination[17] = (int)((uint)source[12] >> 24) | ((source[13] & Mask16) << (24 - 16));
            destination[18] = (int)((uint)source[13] >> 16) | ((source[14] & Mask8) << (24 - 8));
            destination[19] = (int)((uint)source[14] >> 8);
            destination[20] = (int)(((uint)source[15] >> 0) & Mask24);
            destination[21] = (int)((uint)source[15] >> 24) | ((source[16] & Mask16) << (24 - 16));
            destination[22] = (int)((uint)source[16] >> 16) | ((source[17] & Mask8) << (24 - 8));
            destination[23] = (int)((uint)source[17] >> 8);
            destination[24] = (int)(((uint)source[18] >> 0) & Mask24);
            destination[25] = (int)((uint)source[18] >> 24) | ((source[19] & Mask16) << (24 - 16));
            destination[26] = (int)((uint)source[19] >> 16) | ((source[20] & Mask8) << (24 - 8));
            destination[27] = (int)((uint)source[20] >> 8);
            destination[28] = (int)(((uint)source[21] >> 0) & Mask24);
            destination[29] = (int)((uint)source[21] >> 24) | ((source[22] & Mask16) << (24 - 16));
            destination[30] = (int)((uint)source[22] >> 16) | ((source[23] & Mask8) << (24 - 8));
            destination[31] = (int)((uint)source[23] >> 8);
        }

        static void Unpack25(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask25);
            destination[1] = (int)((uint)source[0] >> 25) | ((source[1] & Mask18) << (25 - 18));
            destination[2] = (int)((uint)source[1] >> 18) | ((source[2] & Mask11) << (25 - 11));
            destination[3] = (int)((uint)source[2] >> 11) | ((source[3] & Mask4) << (25 - 4));
            destination[4] = (int)(((uint)source[3] >> 4) & Mask25);
            destination[5] = (int)((uint)source[3] >> 29) | ((source[4] & Mask22) << (25 - 22));
            destination[6] = (int)((uint)source[4] >> 22) | ((source[5] & Mask15) << (25 - 15));
            destination[7] = (int)((uint)source[5] >> 15) | ((source[6] & Mask8) << (25 - 8));
            destination[8] = (int)((uint)source[6] >> 8) | ((source[7] & Mask1) << (25 - 1));
            destination[9] = (int)(((uint)source[7] >> 1) & Mask25);
            destination[10] = (int)((uint)source[7] >> 26) | ((source[8] & Mask19) << (25 - 19));
            destination[11] = (int)((uint)source[8] >> 19) | ((source[9] & Mask12) << (25 - 12));
            destination[12] = (int)((uint)source[9] >> 12) | ((source[10] & Mask5) << (25 - 5));
            destination[13] = (int)(((uint)source[10] >> 5) & Mask25);
            destination[14] = (int)((uint)source[10] >> 30) | ((source[11] & Mask23) << (25 - 23));
            destination[15] = (int)((uint)source[11] >> 23) | ((source[12] & Mask16) << (25 - 16));
            destination[16] = (int)((uint)source[12] >> 16) | ((source[13] & Mask9) << (25 - 9));
            destination[17] = (int)((uint)source[13] >> 9) | ((source[14] & Mask2) << (25 - 2));
            destination[18] = (int)(((uint)source[14] >> 2) & Mask25);
            destination[19] = (int)((uint)source[14] >> 27) | ((source[15] & Mask20) << (25 - 20));
            destination[20] = (int)((uint)source[15] >> 20) | ((source[16] & Mask13) << (25 - 13));
            destination[21] = (int)((uint)source[16] >> 13) | ((source[17] & Mask6) << (25 - 6));
            destination[22] = (int)(((uint)source[17] >> 6) & Mask25);
            destination[23] = (int)((uint)source[17] >> 31) | ((source[18] & Mask24) << (25 - 24));
            destination[24] = (int)((uint)source[18] >> 24) | ((source[19] & Mask17) << (25 - 17));
            destination[25] = (int)((uint)source[19] >> 17) | ((source[20] & Mask10) << (25 - 10));
            destination[26] = (int)((uint)source[20] >> 10) | ((source[21] & Mask3) << (25 - 3));
            destination[27] = (int)(((uint)source[21] >> 3) & Mask25);
            destination[28] = (int)((uint)source[21] >> 28) | ((source[22] & Mask21) << (25 - 21));
            destination[29] = (int)((uint)source[22] >> 21) | ((source[23] & Mask14) << (25 - 14));
            destination[30] = (int)((uint)source[23] >> 14) | ((source[24] & Mask7) << (25 - 7));
            destination[31] = (int)((uint)source[24] >> 7);
        }

        static void Unpack26(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask26);
            destination[1] = (int)((uint)source[0] >> 26) | ((source[1] & Mask20) << (26 - 20));
            destination[2] = (int)((uint)source[1] >> 20) | ((source[2] & Mask14) << (26 - 14));
            destination[3] = (int)((uint)source[2] >> 14) | ((source[3] & Mask8) << (26 - 8));
            destination[4] = (int)((uint)source[3] >> 8) | ((source[4] & Mask2) << (26 - 2));
            destination[5] = (int)(((uint)source[4] >> 2) & Mask26);
            destination[6] = (int)((uint)source[4] >> 28) | ((source[5] & Mask22) << (26 - 22));
            destination[7] = (int)((uint)source[5] >> 22) | ((source[6] & Mask16) << (26 - 16));
            destination[8] = (int)((uint)source[6] >> 16) | ((source[7] & Mask10) << (26 - 10));
            destination[9] = (int)((uint)source[7] >> 10) | ((source[8] & Mask4) << (26 - 4));
            destination[10] = (int)(((uint)source[8] >> 4) & Mask26);
            destination[11] = (int)((uint)source[8] >> 30) | ((source[9] & Mask24) << (26 - 24));
            destination[12] = (int)((uint)source[9] >> 24) | ((source[10] & Mask18) << (26 - 18));
            destination[13] = (int)((uint)source[10] >> 18) | ((source[11] & Mask12) << (26 - 12));
            destination[14] = (int)((uint)source[11] >> 12) | ((source[12] & Mask6) << (26 - 6));
            destination[15] = (int)((uint)source[12] >> 6);
            destination[16] = (int)(((uint)source[13] >> 0) & Mask26);
            destination[17] = (int)((uint)source[13] >> 26) | ((source[14] & Mask20) << (26 - 20));
            destination[18] = (int)((uint)source[14] >> 20) | ((source[15] & Mask14) << (26 - 14));
            destination[19] = (int)((uint)source[15] >> 14) | ((source[16] & Mask8) << (26 - 8));
            destination[20] = (int)((uint)source[16] >> 8) | ((source[17] & Mask2) << (26 - 2));
            destination[21] = (int)(((uint)source[17] >> 2) & Mask26);
            destination[22] = (int)((uint)source[17] >> 28) | ((source[18] & Mask22) << (26 - 22));
            destination[23] = (int)((uint)source[18] >> 22) | ((source[19] & Mask16) << (26 - 16));
            destination[24] = (int)((uint)source[19] >> 16) | ((source[20] & Mask10) << (26 - 10));
            destination[25] = (int)((uint)source[20] >> 10) | ((source[21] & Mask4) << (26 - 4));
            destination[26] = (int)(((uint)source[21] >> 4) & Mask26);
            destination[27] = (int)((uint)source[21] >> 30) | ((source[22] & Mask24) << (26 - 24));
            destination[28] = (int)((uint)source[22] >> 24) | ((source[23] & Mask18) << (26 - 18));
            destination[29] = (int)((uint)source[23] >> 18) | ((source[24] & Mask12) << (26 - 12));
            destination[30] = (int)((uint)source[24] >> 12) | ((source[25] & Mask6) << (26 - 6));
            destination[31] = (int)((uint)source[25] >> 6);
        }

        static void Unpack27(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask27);
            destination[1] = (int)((uint)source[0] >> 27) | ((source[1] & Mask22) << (27 - 22));
            destination[2] = (int)((uint)source[1] >> 22) | ((source[2] & Mask17) << (27 - 17));
            destination[3] = (int)((uint)source[2] >> 17) | ((source[3] & Mask12) << (27 - 12));
            destination[4] = (int)((uint)source[3] >> 12) | ((source[4] & Mask7) << (27 - 7));
            destination[5] = (int)((uint)source[4] >> 7) | ((source[5] & Mask2) << (27 - 2));
            destination[6] = (int)(((uint)source[5] >> 2) & Mask27);
            destination[7] = (int)((uint)source[5] >> 29) | ((source[6] & Mask24) << (27 - 24));
            destination[8] = (int)((uint)source[6] >> 24) | ((source[7] & Mask19) << (27 - 19));
            destination[9] = (int)((uint)source[7] >> 19) | ((source[8] & Mask14) << (27 - 14));
            destination[10] = (int)((uint)source[8] >> 14) | ((source[9] & Mask9) << (27 - 9));
            destination[11] = (int)((uint)source[9] >> 9) | ((source[10] & Mask4) << (27 - 4));
            destination[12] = (int)(((uint)source[10] >> 4) & Mask27);
            destination[13] = (int)((uint)source[10] >> 31) | ((source[11] & Mask26) << (27 - 26));
            destination[14] = (int)((uint)source[11] >> 26) | ((source[12] & Mask21) << (27 - 21));
            destination[15] = (int)((uint)source[12] >> 21) | ((source[13] & Mask16) << (27 - 16));
            destination[16] = (int)((uint)source[13] >> 16) | ((source[14] & Mask11) << (27 - 11));
            destination[17] = (int)((uint)source[14] >> 11) | ((source[15] & Mask6) << (27 - 6));
            destination[18] = (int)((uint)source[15] >> 6) | ((source[16] & Mask1) << (27 - 1));
            destination[19] = (int)(((uint)source[16] >> 1) & Mask27);
            destination[20] = (int)((uint)source[16] >> 28) | ((source[17] & Mask23) << (27 - 23));
            destination[21] = (int)((uint)source[17] >> 23) | ((source[18] & Mask18) << (27 - 18));
            destination[22] = (int)((uint)source[18] >> 18) | ((source[19] & Mask13) << (27 - 13));
            destination[23] = (int)((uint)source[19] >> 13) | ((source[20] & Mask8) << (27 - 8));
            destination[24] = (int)((uint)source[20] >> 8) | ((source[21] & Mask3) << (27 - 3));
            destination[25] = (int)(((uint)source[21] >> 3) & Mask27);
            destination[26] = (int)((uint)source[21] >> 30) | ((source[22] & Mask25) << (27 - 25));
            destination[27] = (int)((uint)source[22] >> 25) | ((source[23] & Mask20) << (27 - 20));
            destination[28] = (int)((uint)source[23] >> 20) | ((source[24] & Mask15) << (27 - 15));
            destination[29] = (int)((uint)source[24] >> 15) | ((source[25] & Mask10) << (27 - 10));
            destination[30] = (int)((uint)source[25] >> 10) | ((source[26] & Mask5) << (27 - 5));
            destination[31] = (int)((uint)source[26] >> 5);
        }

        static void Unpack28(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask28);
            destination[1] = (int)((uint)source[0] >> 28) | ((source[1] & Mask24) << (28 - 24));
            destination[2] = (int)((uint)source[1] >> 24) | ((source[2] & Mask20) << (28 - 20));
            destination[3] = (int)((uint)source[2] >> 20) | ((source[3] & Mask16) << (28 - 16));
            destination[4] = (int)((uint)source[3] >> 16) | ((source[4] & Mask12) << (28 - 12));
            destination[5] = (int)((uint)source[4] >> 12) | ((source[5] & Mask8) << (28 - 8));
            destination[6] = (int)((uint)source[5] >> 8) | ((source[6] & Mask4) << (28 - 4));
            destination[7] = (int)((uint)source[6] >> 4);
            destination[8] = (int)(((uint)source[7] >> 0) & Mask28);
            destination[9] = (int)((uint)source[7] >> 28) | ((source[8] & Mask24) << (28 - 24));
            destination[10] = (int)((uint)source[8] >> 24) | ((source[9] & Mask20) << (28 - 20));
            destination[11] = (int)((uint)source[9] >> 20) | ((source[10] & Mask16) << (28 - 16));
            destination[12] = (int)((uint)source[10] >> 16) | ((source[11] & Mask12) << (28 - 12));
            destination[13] = (int)((uint)source[11] >> 12) | ((source[12] & Mask8) << (28 - 8));
            destination[14] = (int)((uint)source[12] >> 8) | ((source[13] & Mask4) << (28 - 4));
            destination[15] = (int)((uint)source[13] >> 4);
            destination[16] = (int)(((uint)source[14] >> 0) & Mask28);
            destination[17] = (int)((uint)source[14] >> 28) | ((source[15] & Mask24) << (28 - 24));
            destination[18] = (int)((uint)source[15] >> 24) | ((source[16] & Mask20) << (28 - 20));
            destination[19] = (int)((uint)source[16] >> 20) | ((source[17] & Mask16) << (28 - 16));
            destination[20] = (int)((uint)source[17] >> 16) | ((source[18] & Mask12) << (28 - 12));
            destination[21] = (int)((uint)source[18] >> 12) | ((source[19] & Mask8) << (28 - 8));
            destination[22] = (int)((uint)source[19] >> 8) | ((source[20] & Mask4) << (28 - 4));
            destination[23] = (int)((uint)source[20] >> 4);
            destination[24] = (int)(((uint)source[21] >> 0) & Mask28);
            destination[25] = (int)((uint)source[21] >> 28) | ((source[22] & Mask24) << (28 - 24));
            destination[26] = (int)((uint)source[22] >> 24) | ((source[23] & Mask20) << (28 - 20));
            destination[27] = (int)((uint)source[23] >> 20) | ((source[24] & Mask16) << (28 - 16));
            destination[28] = (int)((uint)source[24] >> 16) | ((source[25] & Mask12) << (28 - 12));
            destination[29] = (int)((uint)source[25] >> 12) | ((source[26] & Mask8) << (28 - 8));
            destination[30] = (int)((uint)source[26] >> 8) | ((source[27] & Mask4) << (28 - 4));
            destination[31] = (int)((uint)source[27] >> 4);
        }

        static void Unpack29(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask29);
            destination[1] = (int)((uint)source[0] >> 29) | ((source[1] & Mask26) << (29 - 26));
            destination[2] = (int)((uint)source[1] >> 26) | ((source[2] & Mask23) << (29 - 23));
            destination[3] = (int)((uint)source[2] >> 23) | ((source[3] & Mask20) << (29 - 20));
            destination[4] = (int)((uint)source[3] >> 20) | ((source[4] & Mask17) << (29 - 17));
            destination[5] = (int)((uint)source[4] >> 17) | ((source[5] & Mask14) << (29 - 14));
            destination[6] = (int)((uint)source[5] >> 14) | ((source[6] & Mask11) << (29 - 11));
            destination[7] = (int)((uint)source[6] >> 11) | ((source[7] & Mask8) << (29 - 8));
            destination[8] = (int)((uint)source[7] >> 8) | ((source[8] & Mask5) << (29 - 5));
            destination[9] = (int)((uint)source[8] >> 5) | ((source[9] & Mask2) << (29 - 2));
            destination[10] = (int)(((uint)source[9] >> 2) & Mask29);
            destination[11] = (int)((uint)source[9] >> 31) | ((source[10] & Mask28) << (29 - 28));
            destination[12] = (int)((uint)source[10] >> 28) | ((source[11] & Mask25) << (29 - 25));
            destination[13] = (int)((uint)source[11] >> 25) | ((source[12] & Mask22) << (29 - 22));
            destination[14] = (int)((uint)source[12] >> 22) | ((source[13] & Mask19) << (29 - 19));
            destination[15] = (int)((uint)source[13] >> 19) | ((source[14] & Mask16) << (29 - 16));
            destination[16] = (int)((uint)source[14] >> 16) | ((source[15] & Mask13) << (29 - 13));
            destination[17] = (int)((uint)source[15] >> 13) | ((source[16] & Mask10) << (29 - 10));
            destination[18] = (int)((uint)source[16] >> 10) | ((source[17] & Mask7) << (29 - 7));
            destination[19] = (int)((uint)source[17] >> 7) | ((source[18] & Mask4) << (29 - 4));
            destination[20] = (int)((uint)source[18] >> 4) | ((source[19] & Mask1) << (29 - 1));
            destination[21] = (int)(((uint)source[19] >> 1) & Mask29);
            destination[22] = (int)((uint)source[19] >> 30) | ((source[20] & Mask27) << (29 - 27));
            destination[23] = (int)((uint)source[20] >> 27) | ((source[21] & Mask24) << (29 - 24));
            destination[24] = (int)((uint)source[21] >> 24) | ((source[22] & Mask21) << (29 - 21));
            destination[25] = (int)((uint)source[22] >> 21) | ((source[23] & Mask18) << (29 - 18));
            destination[26] = (int)((uint)source[23] >> 18) | ((source[24] & Mask15) << (29 - 15));
            destination[27] = (int)((uint)source[24] >> 15) | ((source[25] & Mask12) << (29 - 12));
            destination[28] = (int)((uint)source[25] >> 12) | ((source[26] & Mask9) << (29 - 9));
            destination[29] = (int)((uint)source[26] >> 9) | ((source[27] & Mask6) << (29 - 6));
            destination[30] = (int)((uint)source[27] >> 6) | ((source[28] & Mask3) << (29 - 3));
            destination[31] = (int)((uint)source[28] >> 3);
        }

        static void Unpack30(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask30);
            destination[1] = (int)((uint)source[0] >> 30) | ((source[1] & Mask28) << (30 - 28));
            destination[2] = (int)((uint)source[1] >> 28) | ((source[2] & Mask26) << (30 - 26));
            destination[3] = (int)((uint)source[2] >> 26) | ((source[3] & Mask24) << (30 - 24));
            destination[4] = (int)((uint)source[3] >> 24) | ((source[4] & Mask22) << (30 - 22));
            destination[5] = (int)((uint)source[4] >> 22) | ((source[5] & Mask20) << (30 - 20));
            destination[6] = (int)((uint)source[5] >> 20) | ((source[6] & Mask18) << (30 - 18));
            destination[7] = (int)((uint)source[6] >> 18) | ((source[7] & Mask16) << (30 - 16));
            destination[8] = (int)((uint)source[7] >> 16) | ((source[8] & Mask14) << (30 - 14));
            destination[9] = (int)((uint)source[8] >> 14) | ((source[9] & Mask12) << (30 - 12));
            destination[10] = (int)((uint)source[9] >> 12) | ((source[10] & Mask10) << (30 - 10));
            destination[11] = (int)((uint)source[10] >> 10) | ((source[11] & Mask8) << (30 - 8));
            destination[12] = (int)((uint)source[11] >> 8) | ((source[12] & Mask6) << (30 - 6));
            destination[13] = (int)((uint)source[12] >> 6) | ((source[13] & Mask4) << (30 - 4));
            destination[14] = (int)((uint)source[13] >> 4) | ((source[14] & Mask2) << (30 - 2));
            destination[15] = (int)((uint)source[14] >> 2);
            destination[16] = (int)(((uint)source[15] >> 0) & Mask30);
            destination[17] = (int)((uint)source[15] >> 30) | ((source[16] & Mask28) << (30 - 28));
            destination[18] = (int)((uint)source[16] >> 28) | ((source[17] & Mask26) << (30 - 26));
            destination[19] = (int)((uint)source[17] >> 26) | ((source[18] & Mask24) << (30 - 24));
            destination[20] = (int)((uint)source[18] >> 24) | ((source[19] & Mask22) << (30 - 22));
            destination[21] = (int)((uint)source[19] >> 22) | ((source[20] & Mask20) << (30 - 20));
            destination[22] = (int)((uint)source[20] >> 20) | ((source[21] & Mask18) << (30 - 18));
            destination[23] = (int)((uint)source[21] >> 18) | ((source[22] & Mask16) << (30 - 16));
            destination[24] = (int)((uint)source[22] >> 16) | ((source[23] & Mask14) << (30 - 14));
            destination[25] = (int)((uint)source[23] >> 14) | ((source[24] & Mask12) << (30 - 12));
            destination[26] = (int)((uint)source[24] >> 12) | ((source[25] & Mask10) << (30 - 10));
            destination[27] = (int)((uint)source[25] >> 10) | ((source[26] & Mask8) << (30 - 8));
            destination[28] = (int)((uint)source[26] >> 8) | ((source[27] & Mask6) << (30 - 6));
            destination[29] = (int)((uint)source[27] >> 6) | ((source[28] & Mask4) << (30 - 4));
            destination[30] = (int)((uint)source[28] >> 4) | ((source[29] & Mask2) << (30 - 2));
            destination[31] = (int)((uint)source[29] >> 2);
        }

        static void Unpack31(ReadOnlySpan<int> source, Span<int> destination)
        {
            destination[0] = (int)(((uint)source[0] >> 0) & Mask31);
            destination[1] = (int)((uint)source[0] >> 31) | ((source[1] & Mask30) << (31 - 30));
            destination[2] = (int)((uint)source[1] >> 30) | ((source[2] & Mask29) << (31 - 29));
            destination[3] = (int)((uint)source[2] >> 29) | ((source[3] & Mask28) << (31 - 28));
            destination[4] = (int)((uint)source[3] >> 28) | ((source[4] & Mask27) << (31 - 27));
            destination[5] = (int)((uint)source[4] >> 27) | ((source[5] & Mask26) << (31 - 26));
            destination[6] = (int)((uint)source[5] >> 26) | ((source[6] & Mask25) << (31 - 25));
            destination[7] = (int)((uint)source[6] >> 25) | ((source[7] & Mask24) << (31 - 24));
            destination[8] = (int)((uint)source[7] >> 24) | ((source[8] & Mask23) << (31 - 23));
            destination[9] = (int)((uint)source[8] >> 23) | ((source[9] & Mask22) << (31 - 22));
            destination[10] = (int)((uint)source[9] >> 22) | ((source[10] & Mask21) << (31 - 21));
            destination[11] = (int)((uint)source[10] >> 21) | ((source[11] & Mask20) << (31 - 20));
            destination[12] = (int)((uint)source[11] >> 20) | ((source[12] & Mask19) << (31 - 19));
            destination[13] = (int)((uint)source[12] >> 19) | ((source[13] & Mask18) << (31 - 18));
            destination[14] = (int)((uint)source[13] >> 18) | ((source[14] & Mask17) << (31 - 17));
            destination[15] = (int)((uint)source[14] >> 17) | ((source[15] & Mask16) << (31 - 16));
            destination[16] = (int)((uint)source[15] >> 16) | ((source[16] & Mask15) << (31 - 15));
            destination[17] = (int)((uint)source[16] >> 15) | ((source[17] & Mask14) << (31 - 14));
            destination[18] = (int)((uint)source[17] >> 14) | ((source[18] & Mask13) << (31 - 13));
            destination[19] = (int)((uint)source[18] >> 13) | ((source[19] & Mask12) << (31 - 12));
            destination[20] = (int)((uint)source[19] >> 12) | ((source[20] & Mask11) << (31 - 11));
            destination[21] = (int)((uint)source[20] >> 11) | ((source[21] & Mask10) << (31 - 10));
            destination[22] = (int)((uint)source[21] >> 10) | ((source[22] & Mask9) << (31 - 9));
            destination[23] = (int)((uint)source[22] >> 9) | ((source[23] & Mask8) << (31 - 8));
            destination[24] = (int)((uint)source[23] >> 8) | ((source[24] & Mask7) << (31 - 7));
            destination[25] = (int)((uint)source[24] >> 7) | ((source[25] & Mask6) << (31 - 6));
            destination[26] = (int)((uint)source[25] >> 6) | ((source[26] & Mask5) << (31 - 5));
            destination[27] = (int)((uint)source[26] >> 5) | ((source[27] & Mask4) << (31 - 4));
            destination[28] = (int)((uint)source[27] >> 4) | ((source[28] & Mask3) << (31 - 3));
            destination[29] = (int)((uint)source[28] >> 3) | ((source[29] & Mask2) << (31 - 2));
            destination[30] = (int)((uint)source[29] >> 2) | ((source[30] & Mask1) << (31 - 1));
            destination[31] = (int)((uint)source[30] >> 1);
        }

        static void Unpack32(ReadOnlySpan<int> source, Span<int> destination)
        {
            source[0..32].CopyTo(destination);
        }
    }
}