using System.Runtime.CompilerServices;
using System;

namespace DocuSignPoc.Services.DocuSign
{
	public static class CryptographicOperations
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool FixedTimeEquals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
		{
			if (left.Length != right.Length)
				return false;
			int length = left.Length;
			int num = 0;
			for (int index = 0; index < length; ++index)
				num |= (int)left[index] - (int)right[index];
			return num == 0;
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static void ZeroMemory(Span<byte> buffer) => buffer.Clear();
	}
}