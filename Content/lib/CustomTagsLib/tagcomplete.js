(function ($) {

    $.fn.tagComplete = function (userOpts) {

        var defaultOpts = {
            itemKey: "",
            itemValue: "",
            hide: false,
            keyLimit: 1,
            tokenizer: ",",
            ShowTemplate: "", // Template for Populating List
            template: "", // Template for displaying selected data as Tags
            freeInput: true,
            freeEdit: true,
            autocomplete: {
                data: [],
                ajaxOpts: {
                    method: 'GET',
                    dataType: 'json',
                    data: {}
                },
                params: function (value) {
                    return { Query: value, lol: 23 };
                },
                proccessData: function (data) {
                    return data;
                }

            },        
            //triggers when a new tag is added
            onAdd: function (data) {
                return true;
            },//end on Add
            PreAddedTag: { //Auto complete data
                data: [],
                //incase of remote , autocomplete
                //remote options, same as jqery Ajax
                //options
                ajaxOpts: {
                    //url: "",
                    method: 'GET',
                    dataType: 'json',
                    data: {}
                },

                //set remote query parameters
                //for ajax only
                params: function (value) {
                    return { Query: value, lol: 23 };
                },//end params

                
                proccessData: function (data) {
                    return data;
                },
                //ondelete
                //triggers when a tag is deleted
                onDelete: function (data) {
                    return true;
                }//end on delete
            },
        };//end default objects


        //set default options
        var options = $.extend(true, defaultOpts, userOpts);

        //console.log(options);
        //
        allowbackspaceDelete = false;
        //keycode
        backspaceKey = 8;

        //enter Key
        enterKey = 13;


        //destory plugin
        this.destroy = function () {

            //console.log(this.destroy);

            var instanceId = $(this).data("tagcomplete-id");

            //console.log(instanceId);

            if (!instanceId) {
                return false;
            }

            var tagCompleteDom = document.getElementById(instanceId);


            //remove
            $(tagCompleteDom).remove();


            //show original input
            //remove the data id
            $(this).removeData();

            $(this).val("");

            //add new data
            refreshDom = new Date().getTime() + Math.random();

            //set refresh string
            $(this).attr("data-tagcomplete-refresh", refreshDom);

            $(this).addClass("tag-complete-dead").removeClass('hide');
        } //end


        //reinit plugin
        this.reInit = function (userOptions) {

            //destroy plugin first
            this.destroy();

            //if user wishes to change some options
            //then lets merge option changes
            if (userOptions) {
                options = $.extend(true, options, userOptions);
            }//end if

            //then recreate plugin
            return $.fn.tagComplete.call(this, options);

            //console.log(options);
        }//end

        //proccess plugin
        return this.each(function (instanceNo, userInputDom) {

            //set instance to self 
            var self = this

            //set ajax pool to contain all requests
            ajaxPool = [];

            //lets get the input
            userInput = self = $(userInputDom);

            //check if user input has a prefilled data
            var prefilledVaulues = userInput.val() || ""

            //hide the input
            userInput.hide();

            instanceNo = instanceNo + 1;

            instanceId = "";

            var instanceId = instanceNo + (new Date()).getTime() +
                Math.floor(Math.random());

            //main container
            tagCompleteMain = $("<div id='" + instanceId + "' class='tag_complete_main'>" +
                "<div class='tag_complete'>" +
                "<div class='tags_container'></div>" +
                "<input type='text' class='tag_input d-block full-width ' />" +
                "</div>" +
                "<ul class='autocomplete hide'></ul>" +
                "</div>"
            );

            if (options.hide == true) {
                tagCompleteMain.hide()
            }

            //set the instance id
            userInput.attr("data-tagcomplete-id", instanceId);

            //insert the tagCompleteMain right after the
            //user input tag
            tagCompleteMain.insertAfter(userInput);

            //tagComplete
            tagComplete = tagCompleteMain.find(".tag_complete");

            //tag Input
            tagInput = tagComplete.find(".tag_input");

            //tags container
            tagsContainer = tagComplete.find(".tags_container");

            //auto complete
            autoComplete = tagCompleteMain.find(".autocomplete");

            //domSelectors
            //register the dom and instance
            instanceData = {
                userInput: userInput,
                tagCompleteMain: tagCompleteMain,
                tagComplete: tagComplete,
                tagsContainer: tagsContainer,
                tagInput: tagInput,
                autoComplete: autoComplete,
                options: options,
                ajaxPool: ajaxPool
            };//end dom obj

            //instance
            this.instance = self.instance = instanceData;

            //if user has prefilled data, lets add it 
            if (instanceData.options.PreAddedTag.length > 0) {
                //split the prefilled input data using the tokenizer
                //var splitPrefilledData = prefilledVaulues.split(options.tokenizer)
                //loop splitted data and insert it
                $.each(instanceData.options.PreAddedTag, function (index, value) {
                    $.fn.addTag(value[instanceData.options.itemKey], instanceData.options.ShowTemplate(value), instanceData);
                })//end loop

            }//end if prefilled

            //focus input if the tags_container is clicked
            tagComplete.on('click', instanceData, function (e) {
                //focus the tagInput
                e.data.tagInput.focus();
            });//end if

            //tag Input on focus , focus tagComplete
            //container
            tagInput.on("focus", instanceData, function (e) {
                e.data.tagComplete.addClass("is_focused");
            })
                .on("blur", instanceData, function (e) {
                    e.data.tagComplete.removeClass("is_focused");
                });//end tagComplete focus styling


            //if the auto complete child ,li is clicked
            //,then we
            //add tag
            autoComplete.on('click', 'li', instanceData, function (e) {

                //instance
                inst = e.data;

                //id
                var id = $(this).data('id');
                var selectedData = {};
                $.each(options.autocomplete.data, function (dataid, text) {
                    if (text[inst.options.itemKey] == id) {
                        selectedData = text;
                        return false;
                    }
                });//end loop

                if (inst.options.ShowTemplate != "") {
                    debugger;
                    //add tag
                    $.fn.addTag(id, inst.options.ShowTemplate(selectedData), inst);
                    //$.fn.addTag(id, $(this)[0].innerHTML, inst);

                }
                else {
                    $.fn.addTag(id, $(this).text(), inst);
                }
                //refresh User input
                $.fn.refreshUserInput(inst.userInput, inst.tagsContainer, inst.options);

                //empty value
                tagInput.val("");

                //stop ajax requests
                $.fn.abortAjax(ajaxPool);

                //clear autocomplete
                $.fn.clearAutoComplete(inst.autoComplete);
            });//end onclick


            //if the tag close button is clicked
            tagsContainer.on("click", ".tag .close", instanceData, function (e) {

                //instance data
                inst = e.data;

                tag = $(this).parent(".tag");

                //delete tag
                $.fn.deleteTag(tag, inst);
            });//end onTag Close


            //lets listen if enter key is pressed and not empty
            tagInput.on("keyup", instanceData, function (e) {

                //instance data
                instance = e.data;

                //deflate the instance data
                tagCompleteMain = instance.tagCompleteMain;
                tagComplete = instance.tagComplete;
                tagsContainer = instance.tagsContainer;
                self = tagInput = instance.tagInput;
                autoComplete = instance.autoComplete;
                options = instance.options;
                ajaxPool = instance.ajaxPool;

                value = $(this).val();

                //console.log(value);

                keycode = (e.keyCode ? e.keyCode : e.which);

                //if the value is empty , set auto complete to 0
                if (value.length == 0) {
                    $.fn.clearAutoComplete(autoComplete);
                }//end

                //if value is empty and the backspace is pressed
                //lets delete last input and also populate
                //the input with it

                if (options.freeEdit && (value.length == 0 && (keycode == backspaceKey && allowbackspaceDelete))) {

                    //get lastTag
                    lastTagNo = tagsContainer.find('.tag').length;

                    //if empty abort
                    if (lastTagNo == 0) {
                        return false;
                    }//end if

                    lastTagInfo = $.fn.getTagInfo(tagsContainer, lastTagNo);

                    //delete tag
                    $.fn.deleteTag(lastTagInfo.selector, instance);


                    //if free edit and free input is true
                    //use back key to edit tag data
                    if ((options.freeEdit &&
                        options.freeInput) == true) {
                        //update the tags input text to the deleted
                        //also this will help move the cursor to the end
                        tagInput.focus().val(lastTagInfo.text);

                    }//end if

                    //stop exec
                    return self;
                }//end if the key is back key


                //check if enter is pressed &&
                //tag is not empty then add tag
                //if the key too is tokenizer set
                //create tag
                else if ((keycode == enterKey || e.key == options.tokenizer)
                    && value.length > 0
                    && options.freeInput == true) {

                    //trim last tokenizer
                    if (value.endsWith(options.tokenizer)) {
                        value = value.slice(0, value.lastIndexOf(options.tokenizer));
                    }//end if

                    value = $.trim(value);

                    if (value.length == 0) {

                        $(this).val("");

                        return self;
                    }

                    //add tag
                    // key, value, instance
                    $.fn.addTag(value, value, instance);

                    //empty value
                    $(this).val("");

                    return self;
                }//end if enter is pressed


                //user provided autocomplete data
                autoCompeleteData = options.autocomplete.data;

                //matched contents
                matchedData = {};

                //if freeinput is enabled
                if (options.freeInput == true) {
                    //always the first data will be the value
                    matchedData[value] = value;
                }//end if

                //if we are here, then mean auto complete
                //url or local source is set
                //if value limit is less than keyslimit,then
                //abort
                if (value.length < options.keyLimit) {
                    return self;
                }//end if value length is less than keyslimit

                //if data is available
                if (typeof options.autocomplete.data == 'object') {

                    //loop the data to get keywords
                    for (i = 0; i < autoCompeleteData.length; i++) {

                        dataWord = autoCompeleteData[i];

                        //if we dont have a match
                        if (dataWord.indexOf(value) == -1) {
                            continue;
                        }//end if

                        //add to matched dropdown Data
                        //using the word as key prevent
                        //duplicate values
                        matchedData[dataWord] = dataWord;

                    }//end for loop

                    //upldate the drop down first
                    //before the network request
                    $.fn.updateAutoComplete(autoComplete, matchedData);
                }//end if options.data an array or obj

                //dataUrl
                dataUrl = options.autocomplete.ajaxOpts.url;

                //now if we have url set
                if (dataUrl != null) {

                    ajaxOpts = options.autocomplete.ajaxOpts;

                    params = options.autocomplete.params.call(null, value);

                    //set ajax data
                    ajaxOpts.data = $.extend(options.data, params);

                    //console.log(ajaxOpts.data);

                    ajaxReq = $.ajax(ajaxOpts)
                        .done(function (data) {

                            //proccess data
                            proccessedData = options.autocomplete.proccessData.call(null, data);

                            //if empty we will abort
                            if (proccessedData.length == 0) {
                                return false;
                            }//end if

                            //add to the local data
                            $.extend(matchedData, proccessedData);

                            //update auto complete
                            $.fn.updateAutoComplete(autoComplete, matchedData);
                        });//end ajaxReq


                }//end if the url is provided

            });//end onkeyup ...

        });//end on each loop


    };//end jq module name



    /**
     * Module Functions
     */

    /**
     * addTag
     */
    $.fn.addTag = function (tagId, tagText, instance) {
        debugger;
        var existingTags = tagsContainer.find(".tag");
        var allowTagToEnter = true;
        if (existingTags.length > 0) {
            $.each(existingTags, function (id, text) {
                if ($(text).attr("data-id") == tagId) {
                    allowTagToEnter = false;
                    return false;
                }
            });

        }
        if (allowTagToEnter) {
            debugger;
            closeTag = "<span class='close'></span>";
            console.log(tagText);
            tag = $("<span data-id='" + tagId + "' class='tag'>" +
                tagText + closeTag +
                "</span>");

            //trigger onAdd (returns true if success, can also be cancelled)
            if (instance.options.onAdd.call(null, {
                tagId: tagId,
                tagText: tagText
            })) {
                //result is true, append tag
                instance.tagsContainer.append(tag);
            };

            //stop ajax requests
            $.fn.abortAjax(instance.ajaxPool);

            //clear autocomplete
            $.fn.clearAutoComplete(instance.autoComplete);

            //refresh User input
            $.fn.refreshUserInput(instance.userInput,
                instance.tagsContainer,
                instance.options);
            return true;
        }
        else {
            return false;
        }
    };//end add Tag


    //getTag
    $.fn.getTagInfo = function (tagsContainer, child) {

        selector = ".tag:nth-child(" + child + ")";

        tagDom = tagsContainer.find(selector);

        tagText = tagDom.text();

        tagId = tagDom.data('id');

        tagInfo = {
            id: tagId,
            text: tagText,
            selector: tagDom
        }

        return tagInfo;
    }//end getTag


    //update AutoComplete
    $.fn.updateAutoComplete = function (
        autoCompleteDom,
        data) {
        var listData = "";
        instance.options.autocomplete.data = data;
        //lets loop the data
        $.each(data, function (id, text) {
            debugger;
            if (instance.options.template != "") {
                listData += "<li data-id='" + text[instance.options.itemKey] + "'>" + instance.options.template(text) + "</li>";
            }
            else {
                //build list
                listData += "<li data-id='" + text[instance.options.itemKey] + "'>" + text[instance.options.itemValue] + "</li>";
            }
        });//end loop

        //add autocomplete item
        autoCompleteDom.empty().html(listData).removeClass('hide');

        autoCompleteDom.parent('.tag_complete_main')
            .find('.tag_complete')
            .addClass('has_autocomplete');
    };//end updateAutoComplete

    //clear autoComplete
    $.fn.clearAutoComplete = function (autoCompleteDom) {

        autoCompleteDom.addClass('hide').empty();

        autoCompleteDom.parent('.tag_complete_main')
            .find('.tag_complete')
            .removeClass('has_autocomplete');
    }//end clear AutoComplete

    //abort ajaxRequest
    $.fn.abortAjax = function (ajaxPool) {

        if (ajaxPool.length > 0) {
            //stop all ajax pool
            $.each(ajaxPool, function (id, ajaxReq) {
                ajaxReq.abort();
            });
        }//end if ajax pool isnt empty

    };//end abort pool

    /**
     * refreshUserInput
     */
    $.fn.refreshUserInput = function (
        userInput,
        tagsContainer,
        options
    ) {

        //get all the tags data , tokenize it
        tokenizer = options.tokenizer;

        tokenizedData = "";

        //get all tags
        tags = tagsContainer.find(".tag");

        //if no tag, return false
        if (tags.length == 0) {
            return false;
        }//end

        //loop to get keys
        $.each(tags, function (key, tagObj) {

            //lets get the data id
            tagData = $(this).data("id") || $(this).text();

            tokenizedData += tagData + ",";
        });//end loop

        //remove trailing ,
        tokenizedData = tokenizedData.slice(0, tokenizedData.lastIndexOf(","));

        //console.log(tokenizedData);
        userInput.val(tokenizedData);
    };//end refresh userInput

    /**
     * deleteTag
     */
    $.fn.deleteTag = function (tag, instance) {

        //lets get the tag info
        var tagId = $(tag).data("id");
        var tagText = $(tag).text();

        //delete tag
        tag.remove();

        //call on Delete
        //instance.options.onDelete.call(null, {
        //    id: tagId,
        //    text: tagText
        //});

        $(userInput).val($(userInput).val().replace(tagId + ",", ""));
        $(userInput).val($(userInput).val().replace(tagId, ""));

        //refresh User Input
        $.fn.refreshUserInput(instance.userInput, instance.tagsContainer, instance.options);

        return true;
    }//end delete

    /**
     * End Module Functions
     */

}(jQuery));
