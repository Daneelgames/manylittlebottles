using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour {

	public GuiButtonController buttonJournalBody;
	public GuiButtonController buttonTransfer;
	public GuiButtonController buttonCloseJournal;

	public Animator anim;

    public void EnableJournal()
    {
        anim.SetBool("Enabled", true);
    }
}
