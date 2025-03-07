#import <AVFoundation/AVFoundation.h>


extern "C"
{
    void SetAudioSessionCategory()
    {
        AVAudioSession *audioSession = [AVAudioSession sharedInstance];
        NSError *error = nil;
        
        // 오디오 세션 카테고리 설정
        [audioSession setCategory:AVAudioSessionCategoryPlayback error:&error];
        
        if (error) {
            NSLog(@"Failed to set audio session category: %@", error);
        }
    }
}


