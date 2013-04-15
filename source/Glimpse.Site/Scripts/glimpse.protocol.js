(function($) { 
    var formatJson = function(val) {
        var result = '',
            stack = [],
            stackTop = { indent: '' },
            strLen = val.length,
            char = '',
            indentStr = '    ',
            newLine = '\r\n',
            isInString = false;
        
        for (var i = 0; i < strLen; i++) {
            char = val[i];
            
            if (char == '"' && !isInString || isInString) {
                if (char == '"') 
                    isInString = !isInString; 
                     
                result += char;
            }
            else {  
                if (char == '{' || char == '[') {
                    stack.push(stackTop = {
                        isArray: char == '[',
                        isOutterArray: char == '[' && val[i + 1] == '[',
                        indent: stackTop.indent + (!stackTop.isOutterArray ? indentStr : '')
                    }); 
                }
            
                if (char == '}' || (char == ']' && stackTop.isOutterArray))
                    result += newLine + (stack.length > 1 ? stack[stack.length - 2].indent : '');
             
                result += ((char == ']' && !stackTop.isOutterArray) || char == ':' ? ' ' : '') + char + (char == '[' || char == ':' || (char == ',' && stackTop.isArray) ? ' ' : '');
             
                if ((char == ',' && (!stackTop.isArray || stackTop.isOutterArray)) || char == '{' || (char == '[' && stackTop.isOutterArray))
                    result += newLine + stackTop.indent;
            
                if (char == '}' || char == ']') {
                    stack.pop();
                    stackTop = stack[stack.length - 1];
                } 
            }
            
        }

        return result;
    };


    var process = function(scope, data, metadata) {
        var dataPretty = formatJson(JSON.stringify(data)),
            metadataPretty = metadata ? formatJson(JSON.stringify(metadata)) : '';
        
        scope.parent().before('<strong>Data</strong><pre>' + dataPretty + '</pre>' + (metadata ? '<strong>Metadata</strong><pre>' + metadataPretty + '</pre>' : ''));

        glimpse.render.engine.insert(scope, data, metadata); 
    };
    var processFunction = function(scope, heading, callback) {
        scope.parent().before('<strong>' + heading + '</strong><pre>' + callback.toString().replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</pre>');
    };
    var processHeading = function(scope, heading, description) {
        if (heading)
            scope.parent().prev(':first').append(' - ' + heading);
        if (description)
            scope.parent().before('<div>' + description + '</div>');
    };


    var data1 = {
            'Movie': 'Star Wars',
            'Genera/Theme': 'Science Fiction',
            'GlimpseOn': 'True',
            'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
        };
    processHeading($('.sample1'), 'Process object');
    process($('.sample1'), data1); 

            
    var data2 = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', 'quiet'],
        ['Harrison Ford', 'Han Solo', 'Male', '25'],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
        ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', 'selected'],
        ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
        ['Kenny Baker', 'R2-D2', 'Droid', '150']
    ];
    processHeading($('.sample2'), 'Process array with row styling', '<ul><li>Row styling</li></ul>');
    process($('.sample2'), data2); 

    
    var data3 = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', 'ms'],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', 'info'],
        ['Harrison Ford', 'Han Solo - Solo plays a central role in the various Star Wars set after Return of the Jedi. In The Courtship of Princess Leia (1995), he resigns his commission to pursue Leia, whom he eventually marries.', 'Male', '25'],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
        ['Peter Cushing', '*Grand Moff Tarkin*', 'Female', '69'],
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', 'warn'],
        ['Anthony Daniels', '_C-3PO_', 'Droid', '101'],
        ['Kenny Baker', 'R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.', 'Droid', '150'],
        ['Peter Mayhew', 'Chewbacca |(Test)|', 'Wookie', '45'],
        ['Phil Brown', '\\Uncle Owen\\', 'Male', '32', 'error'],
        ['Shelagh Fraser', 'Aunt Beru', 'Female', '29'],
        ['Alex McCrindle', 'General Dodonna', 'Male', '43', 'fail']
    ];
    processHeading($('.sample3'), 'Process array with row/cell styling', '<ul><li>Row styling</li><li>Trim long string content</li><li>Markup support</li></ul>');
    process($('.sample3'), data3); 

    
    var data4 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample4'), 'Process object with nested array');
    process($('.sample4'), data4); 
    

    var data5 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker - Luke Skywalker was a legendary war hero and Jedi who helped defeat the Galactic Empire in the Galactic Civil War and helped found the New Republic, as well as the New Jedi Order.', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample5'), 'Process object with nested array', '<ul><li>Trim long string content of nested content</li></ul>');
    process($('.sample5'), data5); 

    
    var data6 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender'],
                ['Mark Hamill', 'Luke Skywalker', 'Male'],
                ['James Earl Jones', 'Darth Vader', 'Male'],
                ['Harrison Ford', 'Han Solo', 'Male'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female'],
                ['Anthony Daniels', 'C-3PO', 'Droid'],
                ['Kenny Baker', 'R2-D2', 'Droid']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample6'), 'Process object with nested array', '<ul><li>Show summary version of child array with too many items</li><li>When string in nested array is too long, preview trims the amount it shows</li></ul>');
    process($('.sample6'), data6); 

    
    var data7 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker - Luke Skywalker was a legendary war hero and Jedi who helped defeat the Galactic Empire in the Galactic Civil War and helped found the New Republic, as well as the New Jedi Order.', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample7'), 'Process object with nested array', '<ul><li>Trim long string content of nested content</li><li>Show summary version of child array with too many items</li><li>When too many columns exist in nested array, preview trims the amount it shows</li></ul>');
    process($('.sample7'), data7); 

    
    var data8 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo'
        },
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample8'), 'Process object with nested object');
    process($('.sample8'), data8);

    
    var data9 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo',
            'Carrie Fisher': 'Princess Leia Organa',
            'Peter Cushing': 'Grand Moff Tarkin',
            'Alec Guinness': 'Ben Obi-Wan Kenobi',
            'Anthony Daniels': 'C-3PO',
            'Kenny Baker': 'R2-D2'
        },
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample9'), 'Process object with nested object', '<ul><li>Show summary version of child object with too many items</li><li>When string in nested array is too long, preview trims the amount it shows</li></ul>');
    process($('.sample9'), data9); 


    var data10 = [
        ['Type', 'Name', 'Other'],
        ['\tMovie', 'Star Wars', 'Episode IV'],
        ['\t\tGenera/Theme', 'Science Fiction', 'Action'],
        ['\t\tActors/Cast', [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ], 'Cast details pending'],
        ['\tPlot & Description', 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];
    processHeading($('.sample10'), 'Process array with tabs');
    process($('.sample10'), data10); 

    
    var data11 = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ], 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];
    processHeading($('.sample11'), 'Process array with nested array', '<ul><li>Show summary version of child array with too many items</li></ul>');
    process($('.sample11'), data11); 

    
    var data12 = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo'
        }, 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];
    processHeading($('.sample12'), 'Process array with nested object');
    process($('.sample12'), data12); 
    

    var data13 = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo',
            'Carrie Fisher': 'Princess Leia Organa',
            'Peter Cushing': 'Grand Moff Tarkin',
            'Alec Guinness': 'Ben Obi-Wan Kenobi',
            'Anthony Daniels': 'C-3PO',
            'Kenny Baker': 'R2-D2'
        }, 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];
    processHeading($('.sample13'), 'Process array with nested object', '<ul><li>Show summary version of child object with too many items</li></ul>');
    process($('.sample13'), data13); 

    
    var data14 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', { 'Frist Name': 'Luke Skywalker' }, 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', [['Height', 'Weight'], ['213', '433']], 'Male'],
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample14'), 'Process array with nested object with sub nested object/array');
    process($('.sample14'), data14); 

    
    var data15 = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', { 'Frist Name': 'Luke Skywalker' }, 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', [['Height', 'Weight'], ['213', '433']], 'Male'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    processHeading($('.sample15'), 'Process array with nested object with sub nested object/array', '<ul><li>Show summary version of child object with too many items</li></ul>');
    process($('.sample15'), data15);

    
    var data16 = {
        'Movie': [ 'Star Wars', 'Star Treck'],
        'Genera/Theme': [ 'Science Fiction', 'Drama', 'Romance', 'Action', 'Family' ],
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', [ 'Luke Skywalker', 'Mini Skywalker' ], 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', [['Height', 'Weight'], ['213', '433'], ['123', '45']], 'Male'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', [ 'Princess Leia Organa' ], 'Female', '21'],
                ['Peter Cushing', [ 'Grand Moff Tarkin', 'Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin' ], 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': [ 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.' ]
    };
    processHeading($('.sample16'), 'Process array with nested object with sub nested object/array; with raw array', '<ul><li>Show summary version of child object with too many items</li></ul>');
    process($('.sample16'), data16);


    var data17 = [
        ['Actor', 'Character', 'Gender', 'Age', 'Height'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', '1' ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', '4', 'info' ],
        ['Harrison Ford', 'Han Solo - Solo plays a central role in the various Star Wars set after Return of the Jedi. In The Courtship of Princess Leia (1995), he resigns his commission to pursue Leia, whom he eventually marries.', 'Male', '25', '9'],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21', '8'],
        ['Peter Cushing',  [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ], 'Female', '69', '1'],
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', '9' ],
        ['Anthony Daniels',  [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. Darth Vader. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. Darth Vader. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker.', 'Male', '45'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ], 'Droid', '101', '2'],
        ['Kenny Baker', 'R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.', 'Droid', '150', '6']
    ]; 
    var metaData17 = { layout: [
        [ { data : [{ data : 0, key : true, align : 'right' }, { data : 2, align : 'right' }, { data : '{{3}} - {{4}}', align : 'right' }, ], width : '200px' }, { data : 1 } ]
    ]}; 
    processHeading($('.sample17'), 'Process custom layout array with nested array', '<ul><li>Custom layout hows how data can be pivoted, keys identified, aligned, formatted and widths controlled.</li><li>Show summary version of child array with too many items</li></ul>');
    process($('.sample17'), data17, metaData17); 
    

    var data18 = [
        ['Actor', 'Gender', 'Age', 'Height', 'Character'],
        ['Mark Hamill', 'Male', '21', '2', '-- Search for Cataclysmic Variables and pre-CVs with White Dwarfs and \r\n-- very late secondaries. Just uses some simple color cuts from Paula Szkody.  \r\n-- Another simple query that uses math in the WHERE clause  \r\n\r\nSELECT run, \r\n\tcamCol, \r\n\trerun, \r\n\tfield, \r\n\tobjID, \r\n \tra -- Just get some basic quantities \r\nFROM PhotoPrimary	 -- From all primary detections, regardless of class \r\nWHERE u - g < 0.4 \r\n\tand g - r < 0.7 \r\n\tand r - i > 0.4 \r\n\tand i - z > 0.4 -- that meet the color criteria' ],
        ['James Earl Jones', 'Male', '45', '5', '-- Low-z QSO candidates using the color cuts from Gordon Richards. \r\n-- Also a simple query with a long WHERE clause. \r\n\r\nSELECTg,\r\n\trun,\r\n\trerun,\r\n\tcamcol,\r\n\tfield,\r\n\tobjID\r\nFROMGalaxy\r\nWHERE ( (g <= 22)\r\n\tand (u - g >= -0.27)\r\n\tand (u - g < 0.71)\r\n\tand (g - r >= -0.24)\r\n\tand (g - r < 0.35)\r\n\tand (r - i >= -0.27)\r\n\tand (r - i < 0.57)\r\n\tand (i - z >= -0.35)\r\n\tand (i - z < 0.70) )' ],
        ['Carrie Fisher', 'Female', '21', '3', '-- Using object counting and logic in a query. \r\n-- Find all objects similar to the colors of a quasar at 5.5 \r\n\r\nSELECT count(*) as \'total\',\r\n\tsum( case when (Type=3) then 1 else 0 end) as \'Galaxies\',\r\n\tsum( case when (Type=6) then 1 else 0 end) as \'Stars\',\r\n\tsum( case when (Type not in (3,6)) then 1 else 0 end) as \'Other\'\r\nFROM PhotoPrimary	 -- for each object\r\nWHERE (( u - g > 2.0) or (u > 22.3) ) -- apply the quasar color cut.\r\n\tand ( i between 0 and 19 )\r\n\tand ( g - r > 1.0 )\r\n\tand ( (r - i < 0.08 + 0.42 * (g - r - 0.96)) or (g - r > 2.26 ) )\r\n\tand ( i - z < 0.25 )' ], 
        ['Alec Guinness', 'Female', '70', '9', '-- This is a query from Robert Lupton that finds selected neighbors in a given run and \r\n-- camera column. It contains a nested query containing a join, and a join with the \r\n-- results of the nested query to select only those neighbors from the list that meet \r\n-- certain criteria. The nested queries are required because the Neighbors table does \r\n-- not contain all the parameters for the neighbor objects. This query also contains \r\n-- examples of setting the output precision of columns with STR. \r\n\r\nSELECT\r\n\tobj.run, obj.camCol, str(obj.field, 3) as field,\r\n\tstr(obj.rowc, 6, 1) as rowc, str(obj.colc, 6, 1) as colc,\r\n\tstr(dbo.fObj(obj.objId), 4) as id,\r\n\tstr(obj.psfMag_g - 0*obj.extinction_g, 6, 3) as g,\r\n\tstr(obj.psfMag_r - 0*obj.extinction_r, 6, 3) as r,\r\n\tstr(obj.psfMag_i - 0*obj.extinction_i, 6, 3) as i,\r\n\tstr(obj.psfMag_z - 0*obj.extinction_z, 6, 3) as z,\r\n\tstr(60*distance, 3, 1) as D,\r\n\tdbo.fField(neighborObjId) as nfield,\r\n\tstr(dbo.fObj(neighborObjId), 4) as nid,\'new\' as \'new\' \r\nFROM\r\n\t\t(SELECT obj.objId,\r\n\t\t\trun, camCol, field, rowc, colc,\r\n\t\t\tpsfMag_u, extinction_u,\r\n\t\t\tpsfMag_g, extinction_g,\r\n\t\t\tpsfMag_r, extinction_r,\r\n\t\t\tpsfMag_i, extinction_i,\r\n\t\t\tpsfMag_z, extinction_z,\r\n\t\t\tNN.neighborObjId, NN.distance\r\n\t\tFROM PhotoObj as obj\r\n\t\t\tJOIN neighbors as NN on obj.objId = NN.objId\r\n\t\tWHERE 60*NN.distance between 0 and 15 and\r\n\t\t\tNN.mode = 1 and NN.neighborMode = 1 and\r\n\t\t\trun = 756 and camCol = 5 and\r\n\t\t\tobj.type = 6 and (obj.flags & 0x40006) = 0 and\r\n\t\t\tnchild = 0 and obj.psfMag_i < 20 and\r\n\t\t\t(g - r between 0.3 and 1.1 and r - i between -0.1 and 0.6)\r\n\t\t) as obj \r\nJOIN PhotoObj as nobj on nobj.objId = obj.neighborObjId \r\nWHERE\r\n\tnobj.run = obj.run and\r\n\t(abs(obj.psfMag_g - nobj.psfMag_g) < 0.5 or\r\n\tabs(obj.psfMag_r - nobj.psfMag_r) < 0.5 or\r\n\tabs(obj.psfMag_i - nobj.psfMag_i) < 0.5) \r\nORDER BY obj.run, obj.camCol, obj.field ' ]
    ]; 
    var metaData18 = { layout: [
        [ { data : [ { data : 0, key : true, align : 'right' }, { data : 1, align : 'right' }, { data : 2, align : 'right', post : ' ms', className : 'mono' }, { data : 3, align : 'right', pre : 'T+ ', post : ' ms', className : 'mono' } ], width : '200px' }, { data : 4, isCode : true, codeType : 'sql' } ]
    ]};
    processHeading($('.sample18'), 'Process custom layout array', '<ul><li>Custom layout hows how data can be styled, post/pre formatting and code fomatted.</li></ul>');
    process($('.sample18'), data18, metaData18); 
    

    var data19 = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21' ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', 'info' ],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'], 
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70' ]
    ];
    var metaData19 = { layout: [
        [ { data : '{{0}} - ({{1}})', key : true }, { data : 2, width : '20%' }, { data : 3, width : '20%' } ]
    ]};
    processHeading($('.sample19'), 'Process custom layout array with row style');
    process($('.sample19'), data19, metaData19);
    

 
    var data20 = [
        ['Actor', 'Character', 'Gender', 'Age', 'Details' ],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', null ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', null ],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21', [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ]], 
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', null ]
    ];
    var metaData20 = { layout: [
        [ { data : [ { data : '{{0}} - ({{1}})', key : true }, { data : '\t{{2}}' }, { data : '\t{{3}}' } ], width : '250px' }, 
            { data : 4, layout : [ [ { data : 0, width : '30%' }, { data : 1, width : '30%' }, { data : 2, width : '20%' }, { data : 3, width : '20%' } ] ] } 
        ]
    ]}; 
    processHeading($('.sample20'), 'Process custom layout array with sub custom layout array');
    process($('.sample20'), data20, metaData20);
    

    var data21 = [
        ['Actor', 'Character', 'Gender', 'Age', 'Details'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', null ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', [
                ['Person', 'Alias', 'Type', 'Duration'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ]], 
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21', [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ]] , 
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', null ]
    ];
    processHeading($('.sample21'), 'Process custom layout array with targeted sub custom layout array');
    var metaData21 = { layout: [
            [ { data : [ { data : '{{0}} - ({{1}})', key : true }, 
                         { data : '\t{{2}}' }, { data : '\t{{3}}' } ], width : '250px' }, 
                         { data : 4, layout : { 
                            2 : [ 
                                [ { data : [ { data : 0, align : 'right' }, { data : 2, align : 'right', className : 'mono' } ], width : '50%' }, 
                                  { data : [ { data : 1 }, { data : 3, className : 'mono' } ], width : '50%' } ] 
                                ],
                            3 : [ 
                                [ { data : 0, width : '30%', key : true }, { data : 1, width : '30%' }, { data : 2, width : '20%' }, { data : 3, width : '20%' } ] 
                                ] 
                         } }
            ]
        ]};
    process($('.sample21'), data21, metaData21);  
    
    var data22 = {
        'Movie': 'Star Wars',
        'Genera/Theme': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ],
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    var metaData22 = {
        keysHeadings: true
    };
    processHeading($('.sample22'), 'Process object keys as headings');
    process($('.sample22'), data22, metaData22);
     
    var data23 = {
        normal: 'test',
        unnamed: function() { return; },
        unnamedParam: function(a) { return; },
        unnamedParams: function(a, b, c) { return; },
        named: function named() { return; },
        namedPamam: function namedPamam(hello) { return; },
        namedParams: function namedParams(hello, world) { return; }
    };
    processHeading($('.sample23'), 'Process object and render functions');
    process($('.sample23'), data23);
    

    
    var data24EngineA = {
            build: function(data) {
                var val = [];
                for (var key in data)
                    val.push(' &nbsp; &nbsp; - ' + key + ': \'' + data[key] + '\'');
                return '{<br />' + val.join(',<br />') + '<br />}';
            }
        },
        data24EngineB = {
            build: function(data) {
                if (data !== Object(data))
                    return data;
                
                var val = [];
                for (var key in data)
                    val.push(' &nbsp; &nbsp; ^ ' + data[key] + ': \'' + key + '\'');
                return '{<br />' + val.join(',<br />') + '<br />}';
            }
        };
    glimpse.render.engine.register('data24EngineA', data24EngineA);
    glimpse.render.engine.register('data24EngineB', data24EngineB);
    
    
    var data24 = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', { male: true, female: false, other: true }, { isChild: false, isBaby: false, isTeen: false, isAdult: false, isJedi: true } ],
        ['James Earl Jones', 'Darth Vader', { male: true, female: true }, '45', 'info' ]
    ];
    var metaData24 = { layout: [
        [ { data : '{{0}} - ({{1}})', key : true }, { data : 2, width : '20%', engine: 'data24EngineA' }, { data : 3, width : '20%', engine: 'data24EngineB' } ]
    ]};
    processHeading($('.sample24'), 'Process custom layout array with custom rendering engine');
    process($('.sample24'), data24, metaData24);
    processFunction($('.sample24'), 'Custom Engine - data24EngineA', data24EngineA.build);
    processFunction($('.sample24'), 'Custom Engine - data24EngineB', data24EngineA.build);    

    
    var data25Engine = {
        build: function(data) {
            return JSON.stringify(data);
        }
    };
    glimpse.render.engine.register('data25Engine', data25Engine);

    var data25 = {
        'Movie': 'Star Wars',
        'Genera/Theme': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ],
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ],
        'Headlines': {
                'Mark Hamill': [
                    ['Actor', 'Character', 'Gender', 'Age'],
                    ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'], 
                    ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70' ]
                ],
                'James Earl Jones': {
                    'Mark Hamill': 'Luke Skywalker',
                    'James Earl Jones': 'Darth Vader',
                    'Harrison Ford': 'Han Solo',
                    'Carrie Fisher': 'Princess Leia Organa',
                    'Peter Cushing': 'Grand Moff Tarkin',
                    'Alec Guinness': 'Ben Obi-Wan Kenobi',
                    'Anthony Daniels': 'C-3PO',
                    'Kenny Baker': 'R2-D2'
                },
                'Harrison Ford': 'Han Solo',
                'Alec Guinness': {
                    'Harrison Ford': 'Han Solo',
                    'Carrie Fisher': 'Princess Leia Organa',
                    'Peter Cushing': 'Grand Moff Tarkin',
                    'Alec Guinness': 'Ben Obi-Wan Kenobi'
                }
            }
    };
    var metaData25 = {
        layout: {
            'Genera/Theme': { layout: [
                    [ { data : '{{0}} - ({{1}})', key : true }, { data : 2, width : '20%' }, { data : 3, width : '20%' } ]
                ]},
            'Headlines': {
                    keysHeadings: true,
                    layout: {
                       'Alec Guinness': {
                           engine: 'data25Engine'
                       }
                    }
                }
        }
    };
    processHeading($('.sample25'), 'Process custom layout object with custom layout objects/arrays');
    process($('.sample25'), data25, metaData25);
    processFunction($('.sample25'), 'Custom Engine - data25Engine', data25Engine.build);
    

    var data26 = {
        'Movie': 'Star Wars',
        'Genera/Theme': [ 
                ['Actor', 'Character'],
                ['Carrie Fisher', 'Princess Leia Organa'], 
                ['Alec Guinness', 'Ben Obi-Wan Kenobi']
            ], 
        'Actors/Cast': [ 
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet; teams up with other rebels; and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    var metaData26 = {
        suppressHeader: true,
        layout: {
            'Actors/Cast': {
                suppressHeader: true
            },
            'Genera/Theme': {
                suppressHeader: true,
                layout: [[ { data: 0, align: 'right', width: '50%' }, { data: 1 } ]]
            }
        }
    };
    processHeading($('.sample26'), 'Process custom layout object with all heading suppressed');
    process($('.sample26'), data26, metaData26);
    

    var data27 = [
        ['Actor', 'Character', 'Gender', 'Age', 'Children' ],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', null ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', [ 
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ] ],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21', [ 
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ]], 
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', null ]
    ];
    var metaData27 = { layout: [
        [ 
            { data : 0, width : '30%', key : true, title: 'ACTOR' }, { data : 1, width : '30%' }, { data : 2, width : '20%' }, { data : 3, width : '20%' }
        ],
        [
            { data : 4, minDisplay : true, span : 4, suppressHeader : true, forceFull: true, layout : [ [ { data : 0, width : '30%', key : true }, { data : 1, width : '30%' }, { data : 2, width : '20%' }, { data : 3, width : '20%' } ] ] }
        ]
    ]}; 
    processHeading($('.sample27'), 'Simulate parent/child layout');
    process($('.sample27'), data27, metaData27);
    

    var data28 = [ 
        { 'Actor': 'Mark Hamill', 'Character': 'Luke Skywalker', 'Gender': 'Male', 'Age': '21' },
        { 'Character': 'Darth Vader', 'Actor': 'James Earl Jones', 'Gender': 'Male', 'Age': '45', _metadata: { style: 'warn' } },
        { 'Actor': 'Harrison Ford', 'Character': {
                'Mark Hamill': 'Luke Skywalker',
                'James Earl Jones': 'Darth Vader',
                'Harrison Ford': 'Han Solo'
            }, 'Gender': 'Male', 'Age': '25' },
        { 'Actor': 'Carrie Fisher', 'Character': 'Princess Leia Organa', 'Gender': 'Female', 'Age': '21' },
        { 'Actor': 'Peter Cushing', 'Character': [ 
                { 'Actor': 'Mark Hamill', 'Character': 'Luke Skywalker', 'Gender': 'Male', 'Age': '21' },
                { 'Actor': 'James Earl Jones', 'Character': 'Darth Vader', 'Gender': 'Male', 'Age': '45' },
                { 'Actor': 'Harrison Ford', 'Character': 'Han Solo', 'Gender': 'Male', 'Age': '25' },
                { 'Actor': 'Carrie Fisher', 'Character': 'Princess Leia Organa', 'Gender': 'Female', 'Age': '21' },
                { 'Actor': 'Peter Cushing', 'Character': 'Grand Moff Tarkin', 'Gender': 'Female', 'Age': '69' },
                { 'Actor': 'Alec Guinness', 'Character': 'Ben Obi-Wan Kenobi', 'Gender': 'Female', 'Age': '70' },
                { 'Actor': 'Anthony Daniels', 'Character': 'C-3PO', 'Gender': 'Droid', 'Age': '101' },
                { 'Actor': 'Kenny Baker', 'Character': 'R2-D2', 'Gender': 'Droid', 'Age': '150' }
            ], 'Gender': 'Female', 'Age': '69' },
        { 'Actor': 'Alec Guinness', 'Character': 'Ben Obi-Wan Kenobi', 'Gender': 'Female', 'Age': '70' },
        { 'Actor': 'Anthony Daniels', 'Character': 'C-3PO', 'Gender': 'Droid', 'Age': '101' },
        { 'Actor': 'Kenny Baker', 'Character': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker - Luke Skywalker was a legendary war hero and Jedi who helped defeat the Galactic Empire in the Galactic Civil War and helped found the New Republic, as well as the New Jedi Order.', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ], 'Gender': 'Droid', 'Age': '150' }
    ];
    processHeading($('.sample28'), 'Process array with ', '');
    process($('.sample28'), data28); 
    
    //debugger; 
    var data29 = [ 
        { 'aaa': 'Mark Hamill', 'bbb': 'Luke Skywalker', 'ccc': 'Male', 'ddd': '21', 'eee': 'qwe', 'fff': 'Test' },
        { 'bbb': 'Darth Vader', 'aaa': 'James Earl Jones', 'ccc': 'Male', 'ddd': '45', 'eee': 'asd', 'fff': 'Best' }
    ];
    var metaData29 = { layout: [
        [ 
            { data : 'aaa', width : '30%', key : true }, { data : 'bbb', title : 'Character', width : '30%' }, { data : '{{ccc}} _-_ {{ddd}}', title : 'Gender', width : '20%' }, { data : '{{eee}} _-_ {{fff}}', width : '20%' }
        ]
    ]}; 
    processHeading($('.sample29'), 'Process array with ', '');
    process($('.sample29'), data29, metaData29); 

    //Trigger new rendering
    var renderSample = function() {
        var data = $.parseJSON($('.glimpse-sample').val());
        glimpse.render.engine.insert($('.glimpse-sample-output'), data);
    };
    renderSample();
    $('.glimpse-sample-trigger').click(function(e) { 
        try
        {
            renderSample();
        }
        catch(err)
        {
            alert(err);
        } 
    });
    
    $('.glimpse-holder').remove();
    $('.glimpse-open').remove();

})(jQueryGlimpse);