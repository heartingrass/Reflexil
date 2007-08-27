
#region " Imports "
using System;
using System.Collections;
using System.Windows.Forms;
using Mono.Cecil.Cil;
using Mono.Cecil;
using Reflexil.Wrappers;
#endregion

namespace Reflexil.Editors
{

    public partial class GenericOperandReferenceEditor<T, W> : ComboBox, IOperandEditor where W : Reflexil.Wrappers.IWrapper<T>, new()
	{
		
		#region " Fields "
		private ICollection m_referenceditems;
		#endregion
		
		#region " Properties "
		public bool IsOperandHandled(object operand)
		{
			return (operand) is T;
		}
		
		public string Label
		{
			get
			{
				return string.Format("-> {0} reference", typeof(W).Name.Replace("Wrapper", string.Empty));
			}
		}
		#endregion
		
		#region " Methods "
        public GenericOperandReferenceEditor()
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

		public GenericOperandReferenceEditor(ICollection referenceditems) : this()
		{
            this.Dock = DockStyle.Fill;
            this.m_referenceditems = referenceditems;
		}
		
		public void Initialize(MethodDefinition mdef)
		{
			Items.Clear();
			if (mdef.HasBody)
			{
				foreach (T refItem in m_referenceditems)
				{
					W item = new W();
					item.Item = refItem;
					item.MethodDefinition = mdef;
					Items.Add(item);
				}
			}
		}
		
		public Mono.Cecil.Cil.Instruction CreateInstruction(CilWorker worker, OpCode opcode)
		{
			return ((W) SelectedItem).CreateInstruction(worker, opcode);
		}
		
		public void SelectOperand(object operand)
		{
			foreach (W wrapper in Items)
			{
				if ( ((object)wrapper.Item) == operand)
				{
					SelectedItem = wrapper;
				}
			}
		}
		#endregion
		
	}
	
}

