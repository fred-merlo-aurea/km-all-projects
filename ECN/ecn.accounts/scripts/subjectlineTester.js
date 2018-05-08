function setFormValues(){
	document.form.from_email.value = window.opener.document.BlastForm.txtEmailFrom.value;
	document.form.from_name.value = window.opener.document.BlastForm.txtEmailFromName.value;
	document.form.subject_line.value = window.opener.document.BlastForm.EmailSubject.value;
}

function preview() {
	if ((document.form.from_name.value == "") || (document.form.from_email.value == "") || (document.form.subject_line.value == "") || (document.form.define_from.value == "") || (document.form.define_subject.value == "")) {
	   
		 // a required field was left blank
		alert ("All fields are required. Fields left blank will repopulate from previously defined fields.");
		setFormValues();
		 return false;
	
	} else {
		// assign input to local vars
		var my_name = ""; // as string
		var my_email = ""; // as string
		var my_subject = ""; // as string
		var my_define_from = ""; // as integer
		var my_define_subject = ""; // as integer
		var from_name_count = ""; // as integer
		var from_email_count = ""; // as integer
		var subject_count = ""; // as integer
		
		var my_name = document.form.from_name.value;
	 	var my_email = document.form.from_email.value;
	  var my_subject = document.form.subject_line.value;
		
		// Note: Outlook, Outlook Express and Eudora allow user defined
		// characters for both the From and Subject lines
		var my_define_from = document.form.define_from.value;
		var my_define_subject = document.form.define_subject.value;
		
		// vars for character totals
		var from_name_count = my_name.length;
		var from_email_count = my_email.length;
		var subject_count = my_subject.length;
		
		
		// onward!
		
		
		/*
			Results: Show the total characters the user has input for each field
		*/
		document.form.output_from_name_total.value = from_name_count;
		document.form.output_email_total.value = from_email_count;		
		document.form.output_subject_total.value = subject_count;
		
		
		/*
			Results: Outlook & Outlook Express 
		*/
		var my_outlook_name = my_name;
		var my_outlook_subject = my_subject;
		
		// check user defined characters and trim if needed
		if (my_outlook_name > my_define_from) {
			my_outlook_name = my_outlook_name.substr(0,my_define_from);
		}
		if (my_outlook_subject > my_define_subject) {
			my_outlook_subject = my_outlook_subject.substr(0,my_define_subject);
		}
			
		// adjust size of text boxes
		document.form.output_outlook_from_name.size = my_outlook_name.length;		
		document.form.output_outlook_subject.size = my_outlook_subject.length;
		
		// output results
		document.form.output_outlook_from_name.value = my_outlook_name;		
		document.form.output_outlook_subject.value = my_outlook_subject;
		
		/* display defined character integers
		document.form.output_outlook_define_from_total.value = my_define_from; // for QA only?
		document.form.output_outlook_define_subject_total.value = my_define_subject;// for QA only?
		*/
		// End of Outlook & Outlook Express Results
		
		
				
		/*
			Results: AOL 8 - 9
		*/		
		my_aol8_name = my_email.substr(0,15);
		my_aol8_subject = my_subject.substr(0,51);
			
		// adjust size of text boxes
		document.form.output_aol8_from_name.size = my_aol8_name.length;		
		document.form.output_aol8_subject.size = my_aol8_subject.length;
		
		// output results
		document.form.output_aol8_from_name.value = my_aol8_name;		
		document.form.output_aol8_subject.value = my_aol8_subject;
		
		// End of AOL 8 - 9 Results
		
		
				
		/*
			Results: AOL Anywhere
		*/		
		my_aolanywhere_name = my_email.substr(0,16);
		my_aolanywhere_subject = my_subject.substr(0,72);
			
		// adjust size of text boxes
		document.form.output_aolanywhere_from_name.size = my_aolanywhere_name.length;		
		document.form.output_aolanywhere_subject.size = my_aolanywhere_subject.length;
		
		// output results
		document.form.output_aolanywhere_from_name.value = my_aolanywhere_name;		
		document.form.output_aolanywhere_subject.value = my_aolanywhere_subject;
		
		// End of AOL  Results
		
		
				
		/*
			Results: Yahoo
		*/				
		if ((from_name_count > 30) && (subject_count >= 80)) {
			yahoo_from_overage = from_name_count - 30;
			yahoo_subject_limit = 80 - yahoo_from_overage;
			my_yahoo_name = my_name;
			my_yahoo_subject = my_subject.substr(0,yahoo_subject_limit);
		} else {
			my_yahoo_name = my_name;
			my_yahoo_subject = my_subject;
		}
			
		// adjust size of text boxes
		document.form.output_yahoo_from_name.size = from_name_count;		
		document.form.output_yahoo_subject.size = subject_count;
		
		// output results
		document.form.output_yahoo_from_name.value = my_yahoo_name;		
		document.form.output_yahoo_subject.value = my_yahoo_subject;
		
		// End of Yahoo Results
		
		
				
		/*
			Results: Yahoo Mail Beta
		*/				
		my_yahoo_beta_name = my_name.substr(0,27);
		my_yahoo_beta_subject = my_subject.substr(0,70);
			
		// adjust size of text boxes
		document.form.output_yahoo_beta_from_name.size = my_yahoo_beta_name.length;		
		document.form.output_yahoo_beta_subject.size = my_yahoo_beta_subject.length;
		
		// output results
		document.form.output_yahoo_beta_from_name.value = my_yahoo_beta_name;		
		document.form.output_yahoo_beta_subject.value = my_yahoo_beta_subject;
		
		// End of Yahoo Mail Beta Results
		
		
				
		/*
			Results: GMail
		*/				
		my_gmail_name = my_name.substr(0,22);
		my_gmail_subject = my_subject.substr(0,68);
			
		// adjust size of text boxes
		document.form.output_gmail_from_name.size = my_gmail_name.length;		
		document.form.output_gmail_subject.size = my_gmail_subject.length;
		
		// output results
		document.form.output_gmail_from_name.value = my_gmail_name;		
		document.form.output_gmail_subject.value = my_gmail_subject;
		
		// End of GMail Results
		
		
				
		/*
			Results: Hotmail & MSN/Hotmail
		*/				
		if ((from_name_count > 20) && (subject_count >= 45)) {
			hotmail_from_overage = from_name_count - 20;
			hotmail_subject_limit = 45 - hotmail_from_overage;
			my_hotmail_name = my_name;
			my_hotmail_subject = my_subject.substr(0,hotmail_subject_limit);
		} else {
			my_hotmail_name = my_name;
			my_hotmail_subject = my_subject;
		}
			
		// adjust size of text boxes
		document.form.output_hotmail_from_name.size = from_name_count;		
		document.form.output_hotmail_subject.size = subject_count;
		
		// output results
		document.form.output_hotmail_from_name.value = my_hotmail_name;		
		document.form.output_hotmail_subject.value = my_hotmail_subject;
		
		// End of Hotmail & MSN/Hotmail Results
		
		
				
		/*
			Results: Windows Live
		*/				
		my_winlive_name = my_name.substr(0,27);
		my_winlive_subject = my_subject.substr(0,53);
			
		// adjust size of text boxes
		document.form.output_winlive_from_name.size = my_winlive_name.length;		
		document.form.output_winlive_subject.size = my_winlive_subject.length;
		
		// output results
		document.form.output_winlive_from_name.value = my_winlive_name;		
		document.form.output_winlive_subject.value = my_winlive_subject;
		
		// End of Windows Live Results
		
		
		/*
			Results: Eudora
		*/
		var my_eudora_name = my_name;
		var my_eudora_subject = my_subject;
		
		// check user defined characters and trim if needed
		if (my_eudora_name > my_define_from) {
			my_eudora_name = my_eudora_name.substr(0,my_define_from);
		}
		if (my_eudora_subject > my_define_subject) {
			my_eudora_subject = my_eudora_subject.substr(0,my_define_subject);
		}
			
		// adjust size of text boxes
		document.form.output_eudora_from_name.size = my_eudora_name.length;		
		document.form.output_eudora_subject.size = my_eudora_subject.length;
		
		// output results
		document.form.output_eudora_from_name.value = my_eudora_name;		
		document.form.output_eudora_subject.value = my_eudora_subject;
		
		/* display defined character integers
		document.form.output_eudora_define_from_total.value = my_define_from; // for QA only?
		document.form.output_eudora_define_subject_total.value = my_define_subject;// for QA only?
		*/
		// End of Eudora Results
		
		
				
		/*
			Results: Excite
		*/				
		if (from_name_count > 20) {
			my_excite_subject = my_subject.substr(0,80);
			document.form.output_excite_subject.rows = 2;
			document.form.output_excite_subject.cols = 50;
		} else {
			my_excite_subject = my_subject.substr(0,40);
			document.form.output_excite_subject.rows = 1;
			document.form.output_excite_subject.cols = 50;
		}
			
		// adjust size of text box
		document.form.output_excite_from_name.size = from_name_count;	
		
		// output results
		document.form.output_excite_from_name.value = my_name;		
		document.form.output_excite_subject.value = my_excite_subject;
		
		// End of Excite Results
		
		
				
		/*
			Results: Juno
		*/				
		if ((from_name_count > 32) && (subject_count >= 55)) {
			juno_from_overage = from_name_count - 32;
			juno_subject_limit = 55 - juno_from_overage;
			my_juno_name = my_name;
			my_juno_subject = my_subject.substr(0,juno_subject_limit);
		} else {
			my_juno_name = my_name;
			my_juno_subject = my_subject;
		}
			
		// adjust size of text boxes
		document.form.output_juno_from_name.size = from_name_count;		
		document.form.output_juno_subject.size = subject_count;
		
		// output results
		document.form.output_juno_from_name.value = my_juno_name;		
		document.form.output_juno_subject.value = my_juno_subject;
		
		// End of Juno Results		
		
		
				
		/*
			Results: Blackberry
		*/				
		my_bberry_name = my_name.substr(0,11);
		my_bberry_subject = my_subject.substr(0,14);
			
		// adjust size of text boxes
		document.form.output_bberry_from_name.size = my_bberry_name.length;		
		document.form.output_bberry_subject.size = my_bberry_subject.length;
		
		// output results
		document.form.output_bberry_from_name.value = my_bberry_name;		
		document.form.output_bberry_subject.value = my_bberry_subject;
		
		// End of Blackberry Results
		
	}
}

// Show/Hide the Results Layer
function ShowHideLayer() {
	document.getElementById("resultsLayer").style.display="block";
}

// Start Over
function clearForm() {
	document.form.from_name.value = "";
	document.form.from_email.value = "";
	document.form.subject_line.value = "";
	document.form.define_from.value = 20;
	document.form.define_subject.value = 50;
	document.getElementById("resultsLayer").style.display="none";	
}
