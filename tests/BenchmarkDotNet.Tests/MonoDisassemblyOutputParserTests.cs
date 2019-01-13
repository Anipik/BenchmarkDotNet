﻿using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnosers.DisassemblerDataContracts;
using Xunit;

namespace BenchmarkDotNet.Tests
{
    public class MonoDisassemblyOutputParserTests
    {
        [Fact]
        public void CanParseMonoDisassemblyOutputFromMac()
        {
            const string input = @"
CFA: [0] def_cfa: %rsp+0x8
CFA: [0] offset: unknown at cfa-0x8
CFA: [4] def_cfa_offset: 0x10
CFA: [8] offset: %r15 at cfa-0x10
Basic block 0 starting at offset 0xb
Basic block 2 starting at offset 0xb
Basic block 1 starting at offset 0x27
CFA: [2f] def_cfa: %rsp+0x8
Method void BenchmarkDotNet.Samples.CPU.Cpu_Atomics:NoLock () emitted at 0x1027cdf80 to 0x1027cdfb0 (code length 48) [BenchmarkDotNet.Samples.exe]
/var/folders/ld/p9yn04fs3ys6h_dkyxvv95_40000gn/T/.WuSVhL:
(__TEXT,__text) section
chmarkDotNet_Samples_CPU_Cpu_Atomics_NoLock:
0000000000000000	subq	$0x8, %rsp
0000000000000004	movq	%r15, (%rsp)
0000000000000008	movq	%rdi, %r15
000000000000000b	movslq	0x18(%r15), %rax
000000000000000f	incl	%eax
0000000000000011	movl	%eax, 0x18(%r15)
0000000000000015	incl	%eax
0000000000000017	movl	%eax, 0x18(%r15)
000000000000001b	incl	%eax
000000000000001d	movl	%eax, 0x18(%r15)
0000000000000021	incl	%eax
0000000000000023	movl	%eax, 0x18(%r15)
0000000000000027	movq	(%rsp), %r15
000000000000002b	addq	$0x8, %rsp
000000000000002f	retq";

            var expected = new DisassemblyResult()
            {
                Methods = new[]
                {
                    new DisassembledMethod()
                    {
                        Name = "NoLock",
                        Maps = new[]
                        {
                            new Map()
                            {
                                Instructions = new List<SourceCode>
                                {
                                    new SourceCode { TextRepresentation = "subq\t$0x8, %rsp" },

                                    new SourceCode { TextRepresentation = "movq\t%r15, (%rsp)" },
                                    new SourceCode { TextRepresentation = "movq\t%rdi, %r15" },
                                    new SourceCode { TextRepresentation = "movslq\t0x18(%r15), %rax" },

                                    new SourceCode { TextRepresentation = "incl\t%eax" },
                                    new SourceCode { TextRepresentation = "movl\t%eax, 0x18(%r15)" },

                                    new SourceCode { TextRepresentation = "incl\t%eax" },
                                    new SourceCode { TextRepresentation = "movl\t%eax, 0x18(%r15)" },

                                    new SourceCode { TextRepresentation = "incl\t%eax" },
                                    new SourceCode { TextRepresentation = "movl\t%eax, 0x18(%r15)" },

                                    new SourceCode { TextRepresentation = "incl\t%eax" },
                                    new SourceCode { TextRepresentation = "movl\t%eax, 0x18(%r15)" },

                                    new SourceCode { TextRepresentation = "movq\t(%rsp), %r15" },
                                    new SourceCode { TextRepresentation = "addq\t$0x8, %rsp" },
                                    new SourceCode { TextRepresentation = "retq" }
                                }
                            }
                        }
                    }
                }
            };

            Check(input, expected, "NoLock");
        }

        [Fact]
        public void CanParseMonoDisassemblyOutputFromWindows()
        {
            const string input = @"
CFA: [0] def_cfa: %rsp+0x8
CFA: [0] offset: unknown at cfa-0x8
CFA: [4] def_cfa_offset: 0x30
CFA: [8] offset: %rsi at cfa-0x30
CFA: [d] offset: %r14 at cfa-0x28
CFA: [12] offset: %r15 at cfa-0x20
Basic block 0 starting at offset 0x12
Basic block 3 starting at offset 0x12
Basic block 5 starting at offset 0x20
Basic block 4 starting at offset 0x37
Basic block 6 starting at offset 0x4a
Basic block 1 starting at offset 0x52
CFA: [64] def_cfa: %rsp+0x8
Method int BenchmarkDotNet.Samples.My:NoLock () emitted at 000001D748E912E0 to 000001D748E91345 (code length 101) [BenchmarkDotNet.Samples.dll]

/test.o:     file format pe-x86-64


Disassembly of section .text:

0000000000000000 <chmarkDotNet_Samples_My_NoLock>:
   0:	48 83 ec 28          	sub    $0x28,%rsp
   4:	48 89 34 24          	mov    %rsi,(%rsp)
   8:	4c 89 74 24 08       	mov    %r14,0x8(%rsp)
   d:	4c 89 7c 24 10       	mov    %r15,0x10(%rsp)
  12:	45 33 ff             	xor    %r15d,%r15d
  15:	45 33 f6             	xor    %r14d,%r14d
  18:	eb 1d                	jmp    37 <chmarkDotNet_Samples_My_NoLock+0x37>
  1a:	48 8d 64 24 00       	lea    0x0(%rsp),%rsp
  1f:	90                   	nop
  20:	49 8b c7             	mov    %r15,%rax
  23:	49 8b ce             	mov    %r14,%rcx
  26:	ba 02 00 00 00       	mov    $0x2,%edx
  2b:	0f af ca             	imul   %edx,%ecx
  2e:	4c 8b f8             	mov    %rax,%r15
  31:	44 03 f9             	add    %ecx,%r15d
  34:	41 ff c6             	inc    %r14d
  37:	41 83 fe 0d          	cmp    $0xd,%r14d
  3b:	40 0f 9c c6          	setl   %sil
  3f:	48 0f b6 f6          	movzbq %sil,%rsi
  43:	48 8b c6             	mov    %rsi,%rax
  46:	85 c0                	test   %eax,%eax
  48:	75 d6                	jne    20 <chmarkDotNet_Samples_My_NoLock+0x20>
  4a:	44 89 7c 24 18       	mov    %r15d,0x18(%rsp)
  4f:	49 8b c7             	mov    %r15,%rax
  52:	48 8b 34 24          	mov    (%rsp),%rsi
  56:	4c 8b 74 24 08       	mov    0x8(%rsp),%r14
  5b:	4c 8b 7c 24 10       	mov    0x10(%rsp),%r15
  60:	48 83 c4 28          	add    $0x28,%rsp
  64:	c3                   	retq
  65:   90                      nop
  66:   90                      nop
";

            var expected = new DisassemblyResult()
            {
                Methods = new[]
                {
                    new DisassembledMethod()
                    {
                        Name = "NoLock",
                        Maps = new[]
                        {
                            new Map()
                            {
                                Instructions = new List<SourceCode>
                                {
                                    new SourceCode { TextRepresentation = "sub    $0x28,%rsp" },
                                    new SourceCode { TextRepresentation = "mov    %rsi,(%rsp)" },
                                    new SourceCode { TextRepresentation = "mov    %r14,0x8(%rsp)" },
                                    new SourceCode { TextRepresentation = "mov    %r15,0x10(%rsp)" },
                                    new SourceCode { TextRepresentation = "xor    %r15d,%r15d" },
                                    new SourceCode { TextRepresentation = "xor    %r14d,%r14d" },
                                    new SourceCode { TextRepresentation = "jmp    37 <chmarkDotNet_Samples_My_NoLock+0x37>" },
                                    new SourceCode { TextRepresentation = "lea    0x0(%rsp),%rsp" },
                                    new SourceCode { TextRepresentation = "nop" },
                                    new SourceCode { TextRepresentation = "mov    %r15,%rax" },
                                    new SourceCode { TextRepresentation = "mov    %r14,%rcx" },
                                    new SourceCode { TextRepresentation = "mov    $0x2,%edx" },
                                    new SourceCode { TextRepresentation = "imul   %edx,%ecx" },
                                    new SourceCode { TextRepresentation = "mov    %rax,%r15" },
                                    new SourceCode { TextRepresentation = "add    %ecx,%r15d" },
                                    new SourceCode { TextRepresentation = "inc    %r14d" },
                                    new SourceCode { TextRepresentation = "cmp    $0xd,%r14d" },
                                    new SourceCode { TextRepresentation = "setl   %sil" },
                                    new SourceCode { TextRepresentation = "movzbq %sil,%rsi" },
                                    new SourceCode { TextRepresentation = "mov    %rsi,%rax" },
                                    new SourceCode { TextRepresentation = "test   %eax,%eax" },
                                    new SourceCode { TextRepresentation = "jne    20 <chmarkDotNet_Samples_My_NoLock+0x20>" },
                                    new SourceCode { TextRepresentation = "mov    %r15d,0x18(%rsp)" },
                                    new SourceCode { TextRepresentation = "mov    %r15,%rax" },
                                    new SourceCode { TextRepresentation = "mov    (%rsp),%rsi" },
                                    new SourceCode { TextRepresentation = "mov    0x8(%rsp),%r14" },
                                    new SourceCode { TextRepresentation = "mov    0x10(%rsp),%r15" },
                                    new SourceCode { TextRepresentation = "add    $0x28,%rsp" },
                                    new SourceCode { TextRepresentation = "retq" },
                                }
                            }
                        }
                    }
                }
            };

            Check(input, expected, "NoLock");
        }

        [Fact]
        public void CanParseMonoDisassemblyOutputFromWindowsWithoutTools()
        {
            const string input = @"
Basic block 0 starting at offset 0xd
Basic block 3 starting at offset 0xd
Basic block 5 starting at offset 0x18
Basic block 4 starting at offset 0x1c
Basic block 6 starting at offset 0x21
Basic block 1 starting at offset 0x24
CFA: [31] def_cfa: %rsp+0x8
Method int BenchmarkDotNet.Samples.Intro.IntroDisasm:Foo () emitted at 0000027E7E4E12E0 to 0000027E7E4E1312 (code length 50) [BenchmarkDotNet.Samples.dll]
'as' is not recognized as an internal or external command,
operable program or batch file.
'x86_64-w64-mingw32-objdump.exe' is not recognized as an internal or external command,
operable program or batch file.
";

            var expected = new DisassemblyResult
            {
                Methods = new[]
                {
                    new DisassembledMethod
                    {
                        Name = "Foo",
                        Maps = new[]
                        {
                            new Map
                            {
                                Instructions = input
                                    .Split('\r', '\n')
                                    .Where(line => !string.IsNullOrWhiteSpace(line))
                                    .Select(line => new SourceCode { TextRepresentation = line })
                                    .ToList()
                            }
                        }
                    }
                },
                Errors = new[]
                {
                    @"It's impossible to get Mono disasm because you don't have some required tools:
'as' is not recognized as an internal or external command
'x86_64-w64-mingw32-objdump.exe' is not recognized as an internal or external command"
                }
            };

            Check(input, expected, "Foo");
        }
        
        [Fact]
        public void CanParseInvalidMonoDisassemblyOutput()
        {
            const string input = @"lalala";

            var expected = new DisassemblyResult
            {
                Methods = new[]
                {
                    new DisassembledMethod
                    {
                        Name = "Foo",
                        Maps = new[]
                        {
                            new Map
                            {
                                Instructions = input
                                    .Split('\r', '\n')
                                    .Where(line => !string.IsNullOrWhiteSpace(line))
                                    .Select(line => new SourceCode { TextRepresentation = line })
                                    .ToList()
                            }
                        }
                    }
                },
                Errors = new[]
                {
                    @"It's impossible to find assembly instructions in the mono output"
                }
            };

            Check(input, expected, "Foo");
        }


        private static void Check(string input, DisassemblyResult expected, string methodName)
        {
            var disassemblyResult = MonoDisassembler.OutputParser.Parse(
                input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None),
                methodName, commandLine: string.Empty);

            Assert.Equal(expected.Methods.Single().Name, disassemblyResult.Methods.Single().Name);
            Assert.Equal(expected.Methods[0].Maps[0].Instructions.Count, disassemblyResult.Methods[0].Maps[0].Instructions.Count);

            for (int i = 0; i < expected.Methods[0].Maps[0].Instructions.Count; i++)
                Assert.Equal(expected.Methods[0].Maps[0].Instructions[i].TextRepresentation,
                    disassemblyResult.Methods[0].Maps[0].Instructions[i].TextRepresentation);
            
            Assert.Equal(expected.Errors.Length, disassemblyResult.Errors.Length);
            for (int i = 0; i < expected.Errors.Length; i++)
                Assert.Equal(expected.Errors[i].Replace("\r", "").Replace("\n", ""), disassemblyResult.Errors[i].Replace("\r", "").Replace("\n", ""));
        }
    }
}